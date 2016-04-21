using Mongo2DocumentDB.Models;

namespace Mongo2DocumentDB
{
    public interface IEmailAccountProvider
    {
        EmailAccount GetEmailAccount(string emailAddress);
    }
}