using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PerformanceTestUtil
{
    public class PerformanceTest
    {
        public static void StartStatisticsBatch(string name, FileInfo statisticsFile)
        {
            File.AppendAllLines(statisticsFile.FullName, new[]
            {
                $"### Batch '{name}' ({DateTime.Now}) ###",
                "Name;Result count;Elapsed seconds"
            });
        }

        public static void EndStatisticsBatch(FileInfo statisticsFile)
        {
            File.AppendAllLines(statisticsFile.FullName, new[] { "###", "" });
        }

        public static void Run(string name, DirectoryInfo resultsFolder, FileInfo statisticsFile, IQuery query)
        {
            var resultsFile = Path.Combine(
                resultsFolder.FullName,
                name + "_" + DateTime.Now.ToString("yyyy-MM-dd_HHmmss") + "_results.txt");

            Console.WriteLine("Executing performance test '{0}'...", name);

            var stopwatch = Stopwatch.StartNew();
            var results = query.Run();
            stopwatch.Stop();

            File.WriteAllText(resultsFile, results.ResultsAsJsonString);

            var totalSeconds = stopwatch.Elapsed.TotalSeconds;

            Console.WriteLine("Finished after {0} seconds.", totalSeconds);
            Console.WriteLine("{0} results returned, dump written to '{1}'.", results.ResultsCount, resultsFile);

            File.AppendAllLines(statisticsFile.FullName, new [] { string.Join("; ", name, results.ResultsCount, totalSeconds)});
        }
    }
}
