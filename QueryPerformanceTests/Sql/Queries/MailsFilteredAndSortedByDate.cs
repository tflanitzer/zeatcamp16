using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Converters;

namespace Sql.Queries
{
    public class MailsFilteredAndSortedByDate : QueryBase
    {
        public MailsFilteredAndSortedByDate(SqlConnection connection) : base(connection) {}

        public override string CommandText => "SELECT * FROM Mail WHERE Date >= '2000-11-01' AND Date < '2000-11-10' ORDER BY Date ASC";
    }
}