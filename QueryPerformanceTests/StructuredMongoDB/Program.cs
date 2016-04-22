using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceTestUtil;
using StructuredMongoDB.Queries;

namespace StructuredMongoDB
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("USAGE: [CommandName] [ResultsFolder] [StatisticsFile]");
                return;
            }

            var resultsFolder = new DirectoryInfo(args[0]);
            var statisticsFile = new FileInfo(args[1]);

            var collection = OpenCollection();
            
            var queries = new Dictionary<string, IQuery>()
            {
                { "MailsFilteredAndSortedByDate", new MailsFilteredAndSortedByDate(collection) },
                { "SentMailCountPerEmailAccount", new SentMailCountPerEmailAccount(collection) },
                { "AllReceivedMailsForAnEmailAccount", new AllReceivedMailsForAnEmailAccount(collection) },
                { "AllMailsWithWordInSubjectOrBody", new AllMailsWithWordInSubjectOrBody(collection) },
                { "ComplexMailQuery", new ComplexMailQuery(collection) },
            };

            PerformanceTest.StartStatisticsBatch("Structured MongoDB", statisticsFile);
            foreach (var query in queries)
            {
                PerformanceTest.Run(query.Key, resultsFolder, statisticsFile, query.Value);
            }
            PerformanceTest.EndStatisticsBatch(statisticsFile);
        }

        private static IMongoCollection<BsonDocument> OpenCollection()
        {
            var mongoClient = new MongoClient(ConfigurationManager.ConnectionStrings["StructuredMongoDB"].ConnectionString);
            var database = mongoClient.GetDatabase("sNoSql");
            var collection = database.GetCollection<BsonDocument>("fatMail");
            
            return collection;
        }
    }
}
