using Mongo2SQL.Models;

namespace Mongo2SQL
{
    public interface IEmailAccountProvider
    {
        EmailAccount GetEmailAccount(string emailAddress);
    }
}