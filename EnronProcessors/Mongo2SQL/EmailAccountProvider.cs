using System.Data;
using System.Data.Entity;
using System.Linq;
using Mongo2SQL.Models;

namespace Mongo2SQL
{
    class EmailAccountProvider : IEmailAccountProvider
    {
        public string DestinationConnectionStringName { get; private set; }

        public EmailAccountProvider(string destinationConnectionStringName)
        {
            DestinationConnectionStringName = destinationConnectionStringName;
        }

        public EmailAccount GetEmailAccount(string emailAddress)
        {
            return GetOrCreateEmailAccount(emailAddress);
        }

        private EmailAccount GetOrCreateEmailAccount(string emailAddress)
        {
            using (var enronSqlContext = new EnronSqlContext(DestinationConnectionStringName))
            using (var transaction = enronSqlContext.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                var existingEmailAccount =
                    enronSqlContext.EmailAccount.SingleOrDefault(_ => _.EmailAddress == emailAddress);

                if (existingEmailAccount != null)
                    return existingEmailAccount;

                var newEmailAccount = new EmailAccount()
                {
                    EmailAddress = emailAddress
                };

                enronSqlContext.EmailAccount.Add(newEmailAccount);

                enronSqlContext.SaveChanges();
                transaction.Commit();

                return newEmailAccount;
            }
        }
    }
}