using Mongo2SQL.Models;

namespace Mongo2SQL
{
    class LockingEmailAccountProviderDecorator : IEmailAccountProvider
    {
        public IEmailAccountProvider Decoratee { get; private set; }

        private readonly object _lockObject;

        public LockingEmailAccountProviderDecorator(IEmailAccountProvider decoratee)
        {
            Decoratee = decoratee;
            _lockObject = new object();
        }

        public EmailAccount GetEmailAccount(string emailAddress)
        {
            lock (_lockObject)
            {
                return Decoratee.GetEmailAccount(emailAddress);
            }
        }
    }
}