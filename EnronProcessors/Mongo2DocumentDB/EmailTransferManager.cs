using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading;
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

        public void TransferAll()
        {
            var stopwatch = Stopwatch.StartNew();
            var cursor = 0;

            var sourceCollection = OpenSourceCollection();

            while (true)
            {
                //using (var destinationContext = new EnronSqlContext(DestinationSqlConnectionStringName))
                //{
                //    destinationContext.Configuration.AutoDetectChangesEnabled = false;

                //    try
                //    {
                //        var batch = sourceCollection.Skip(cursor).Limit(Configuration.BatchSize).ToEnumerable();

                //        if (!batch.Any())
                //            break;

                //        var convertedMails = batch
                //            //.AsParallel()
                //            .Select(originalEmail => EmailConverter.ConvertEmail(originalEmail))
                //            .ToArray();

                //        destinationContext.BulkInsert(convertedMails);

                //        cursor += Configuration.BatchSize;

                //        Console.WriteLine(
                //            "Converted {0} messages ({1} per minute)",
                //            cursor,
                //            cursor/stopwatch.Elapsed.TotalMilliseconds*1000*60);
                //    }
                //    catch (Exception e)
                //    {
                //        Console.WriteLine("Exception occurred (retrying in 10s):" + e.ToString());
                //        Thread.Sleep(TimeSpan.FromSeconds(10));
                //    }
                //}
            }
        }

        private IFindFluent<BsonDocument, BsonDocument> OpenSourceCollection()
        {
            var mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings[SourceMongoConnectionStringName].ConnectionString);
            var database = mongoClient.GetDatabase("test");
            var sourceCollection = database.GetCollection<BsonDocument>("messages");

            return sourceCollection.Find(Builders<BsonDocument>.Filter.Regex("mailbox", new BsonRegularExpression(Configuration.MailBoxMatchPattern, "i")));
        }
    }
}