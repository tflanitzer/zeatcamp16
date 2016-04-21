using System.Collections.Generic;

namespace Mongo2DocumentDB
{
    public class CachingEmailAccountProviderDecorator : IEmailAccountProvider
    {
        public IEmailAccountProvider Decoratee { get; private set; }

        ////private readonly Dictionary<string, EmailAccount> _emailAccountCache;

        //public CachingEmailAccountProviderDecorator(IEmailAccountProvider decoratee)
        //{
        //    Decoratee = decoratee;
        //    _emailAccountCache = new Dictionary<string, EmailAccount>();
        //}

        //public EmailAccount GetEmailAccount(string emailAddress)
        //{
        //    if (_emailAccountCache.ContainsKey(emailAddress))
        //    {
        //        return _emailAccountCache[emailAddress];
        //    }

        //    var emailAccount = Decoratee.GetEmailAccount(emailAddress);
        //    _emailAccountCache[emailAddress] = emailAccount;

        //    return emailAccount;
        //}
    }
}