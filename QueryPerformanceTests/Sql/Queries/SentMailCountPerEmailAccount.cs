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
    public class SentMailCountPerEmailAccount : QueryBase
    {
        public SentMailCountPerEmailAccount(SqlConnection connection) : base(connection) {}

        public override string CommandText => @"
SELECT EmailAccount.Id, EmailAccount.EmailAddress, COUNT(Sender.MailId)
	FROM EmailAccount
		JOIN Sender ON Sender.EmailAccountId = EmailAccount.Id
	GROUP BY EmailAccount.Id, EmailAccount.EmailAddress
	ORDER BY COUNT(Sender.MailId) DESC
";
    }
}