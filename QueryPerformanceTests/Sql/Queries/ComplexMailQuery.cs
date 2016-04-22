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
    public class ComplexMailQuery : QueryBase
    {
        public ComplexMailQuery(SqlConnection connection) : base(connection) {}

        public override string CommandText => @"
SELECT * 
	FROM Mail
		JOIN Sender ON Sender.MailId = Mail.Id
		JOIN EmailAccount AS SenderEmailAccount ON SenderEmailAccount.Id = Sender.EmailAccountId
		JOIN Recipient ON Recipient.MailId = Mail.Id
		JOIN EmailAccount AS RecipientEmailAccount ON RecipientEmailAccount.Id = Recipient.EmailAccountId
	WHERE SenderEmailAccount.EmailAddress = 'jason.bass2@compaq.com'
		AND RecipientEmailAccount.EmailAddress = 'eric.bass@enron.com'
		AND Mail.[Date] >= '2000-11-1'
		AND Mail.[Date] <= '2000-11-30'
		AND (CONTAINS(Body, 'Ramiro') OR CONTAINS(Subject, 'Ramiro'))
";
    }
}