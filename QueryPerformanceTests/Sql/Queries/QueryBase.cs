using System.Data;
using System.Data.SqlClient;
using Newtonsoft.Json;
using PerformanceTestUtil;

namespace Sql.Queries
{
    public abstract class QueryBase : IQuery
    {
        public SqlConnection Connection { get; set; }

        public abstract string CommandText { get; }

        public QueryBase(SqlConnection connection)
        {
            Connection = connection;
        }

        public QueryResult Run()
        {
            var command = Connection.CreateCommand();

            command.CommandText = "SELECT * FROM Mail WHERE Date >= '2001-1-10' AND Date <= '2001-2-4' ORDER BY Date ASC";
            using (var dataReader = command.ExecuteReader())
            {
                var dataTable = new DataTable();
                dataTable.Load(dataReader);

                var resultString = JsonConvert.SerializeObject(dataTable);

                return new QueryResult { ResultsAsJsonString = resultString, ResultsCount = dataTable.Rows.Count };
            }
        }

    }
}