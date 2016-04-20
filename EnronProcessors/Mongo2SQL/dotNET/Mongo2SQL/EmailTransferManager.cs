using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Threading;
using EntityFramework.BulkInsert.Extensions;
using Mongo2SQL.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Mongo2SQL
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

        public void TransferAll()
        {
            var stopwatch = Stopwatch.StartNew();
            var cursor = 0;

            var sourceCollection = OpenSourceCollection();

            while (true)
            {
                using (var destinationContext = new EnronSqlContext(DestinationSqlConnectionStringName))
                {
                    destinationContext.Configuration.AutoDetectChangesEnabled = false;

                    try
                    {
                        var batch = sourceCollection.AsQueryable().Skip(cursor).Take(Configuration.BatchSize);

                        if (!batch.Any())
                            break;

                        var convertedMails = batch
                            .AsParallel()
                            //.AsEnumerable()
                            .Select(originalEmail => EmailConverter.ConvertEmail(originalEmail))
                            .ToArray();

                        //destinationContext.Mail.AddRange(convertedMails);
                        destinationContext.BulkInsert(convertedMails);

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