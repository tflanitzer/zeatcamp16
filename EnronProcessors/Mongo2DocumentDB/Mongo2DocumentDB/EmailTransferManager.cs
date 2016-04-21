using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using Mongo2DocumentDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo2DocumentDB
{
    public class EmailTransferManager
    {
        public string SourceMongoConnectionStringName { get; private set; }
        public string DestinationSqlConnectionStringName { get; private set; }
        public EmailConverter EmailConverter { get; private set; }
        public Configuration Configuration { get; private set; }

        public EmailTransferManager(
            string sourceMongoConnectionStringName,
            string destinationSqlConnectionStringName,
            EmailConverter emailConverter,
            Configuration configuration)
        {
            SourceMongoConnectionStringName = sourceMongoConnectionStringName;
            DestinationSqlConnectionStringName = destinationSqlConnectionStringName;
            EmailConverter = emailConverter;
            Configuration = configuration;
        }

        public async void TransferAll()
        {
            var stopwatch = Stopwatch.StartNew();
            var cursor = 0;

            var sourceCollection = OpenSourceCollection();

            while (true)
            {
                using (var documentClient = CreateDocumentClient())
                {
                    try
                    {
                        var database = await GetOrCreateDatabaseAsync("enron", documentClient);

                        var mailCollection = await documentClient.CreateDocumentCollectionAsync(
                            database.SelfLink,
                            new DocumentCollection {Id = "mail"});


                        var batch = sourceCollection.AsQueryable().Skip(cursor).Take(Configuration.BatchSize);

                        if (!batch.Any())
                            break;


                        //Parallel.ForEach(batch, originalEmail => EmailConverter.ConvertEmail(originalEmail));                 

                        var convertedMails = batch
                            .AsParallel()
                            //.AsEnumerable()
                            .Select(originalEmail => EmailConverter.ConvertEmail(originalEmail))
                            .ToArray();

                        foreach (var convertedMail in convertedMails)
                        {
                            await documentClient.CreateDocumentAsync(mailCollection.Resource.SelfLink, convertedMail);
                        }

                        //destinationContext.Mail.AddRange(convertedMails);

                        //destinationContext.SaveChanges();

                        cursor += Configuration.BatchSize;

                        Console.WriteLine(
                            "Converted {0} messages ({1} per minute)",
                            cursor,
                            cursor/stopwatch.Elapsed.TotalMilliseconds*1000*60);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Exception occurred (retrying in 10s):" + e.ToString());
                        Thread.Sleep(TimeSpan.FromSeconds(10));
                    }
                }
            }
        }

        private static DocumentClient CreateDocumentClient()
        {
            return new DocumentClient(
                new Uri("https://enron.documents.azure.com:443/"),
                "otq7jJqWDqlhchDOpUIGP5MaLUvZUtMaqcDDv47ZBNTqZS9pmsB66hMqUuYQ1wGNw374IRKFSISWYB9UztOweQ==");
        }

        private static async Task<Database> GetOrCreateDatabaseAsync(string id, DocumentClient documentClient)
        {
            // Get the database by name, or create a new one if one with the name provided doesn't exist.
            // Create a query object for database, filter by name.
            IEnumerable<Database> query = from db in documentClient.CreateDatabaseQuery()
                                          where db.Id == id
                                          select db;

            // Run the query and get the database (there should be only one) or null if the query didn't return anything.
            // Note: this will run synchronously. If async exectution is preferred, use IDocumentServiceQuery<T>.ExecuteNextAsync.
            Database database = query.FirstOrDefault();
            if (database == null)
            {
                // Create the database.
                database = await documentClient.CreateDatabaseAsync(new Database { Id = id });
            }

            return database;
        }

        private IEnumerable<BsonDocument> OpenSourceCollection()
        {
            var mongoClient =
                new MongoClient(ConfigurationManager.ConnectionStrings[SourceMongoConnectionStringName].ConnectionString);
            var database = mongoClient.GetDatabase("test");
            var sourceCollection = database.GetCollection<BsonDocument>("messages");

            return sourceCollection.Find(Builders<BsonDocument>.Filter.Regex("mailbox", new BsonRegularExpression(Configuration.MailBoxMatchPattern, "i"))).ToEnumerable();
        }
    }
}