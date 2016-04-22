using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PerformanceTestUtil;
using Sql.Queries;

namespace Sql
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

            using (var sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["EnronSQL"].ConnectionString))
            {
                sqlConnection.Open();

                var queries = new Dictionary<string, IQuery>()
                {
                    { "MailsFilteredAndSortedByDate", new MailsFilteredAndSortedByDate(sqlConnection) },
                    { "SentMailCountPerEmailAccount", new SentMailCountPerEmailAccount(sqlConnection) },
                    { "AllReceivedMailsForAnEmailAccount", new AllReceivedMailsForAnEmailAccount(sqlConnection) },
                    { "AllMailsWithWordInSubjectOrBodyWithContains", new AllMailsWithWordInSubjectOrBodyWithContains(sqlConnection) },
                    { "AllMailsWithWordInSubjectOrBodyWithLike", new AllMailsWithWordInSubjectOrBodyWithLike(sqlConnection) },
                    { "ComplexMailQuery", new ComplexMailQuery(sqlConnection) },
                };

                PerformanceTest.StartStatisticsBatch("SQL Server", statisticsFile);

                foreach (var query in queries)
                {
                    PerformanceTest.Run(query.Key, resultsFolder, statisticsFile, query.Value);
                }
                PerformanceTest.EndStatisticsBatch(statisticsFile);
            }
        }
    }
}
