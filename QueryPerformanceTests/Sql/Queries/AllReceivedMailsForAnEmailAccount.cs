using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using PerformanceTestUtil;

namespace Sql.Queries
{
    public class AllReceivedMailsForAnEmailAccount : QueryBase
    {
        public AllReceivedMailsForAnEmailAccount(SqlConnection connection) : base(connection) {}

        public override string CommandText => @"
SELECT * FROM Mail
	JOIN Recipient ON Recipient.MailId = Mail.Id
	JOIN EmailAccount ON EmailAccount.Id = Recipient.EmailAccountId
	WHERE EmailAccount.EmailAddress = 'eric.bass@enron.com'
";
    }
}