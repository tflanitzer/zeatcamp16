namespace Mongo2DocumentDB
{
    class Program
    {
        static void Main(string[] args)
        {
            // ######
            // NOTE: This application is work in progress and is currently NOT FUNCTIONAL!
            // ######

            var emailAccountProvider = (EmailAccountProvider)null;//new LockingEmailAccountProviderDecorator(
                //new CachingEmailAccountProviderDecorator(
                //    new EmailAccountProvider("SQLDestination"));//);

            var emailConverter = new EmailConverter(emailAccountProvider);

            var emailTransferManager = new EmailTransferManager(
                "MongoDBSource",
                "SQLDestination",
                emailConverter,
                new Configuration());

            emailTransferManager.TransferAll();
        }
    }
}
