using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mongo2SQL.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo2SQL
{
    class Program
    {
        static void Main(string[] args)
        {            
            var emailAccountProvider = new LockingEmailAccountProviderDecorator(
                new CachingEmailAccountProviderDecorator(
                    new EmailAccountProvider("SQLDestination")));

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
