using Mongo2SQL.Models;

namespace Mongo2SQL
{
    class EmailAccountProvider : IEmailAccountProvider
    {
        public EnronSqlContext EnronSqlContext { get; private set; }

        public EmailAccountProvider(EnronSqlContext enronSqlContext)
        {
            EnronSqlContext = enronSqlContext;
        }

        public EmailAccount GetEmailAccount(string emailAddress)
        {
            return new EmailAccount()
            {
                EmailAddress = emailAddress
            };
        }
    }
}