using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            var mongoClient = new MongoClient("mongodb://ub15-zeatcamp16.westeurope.cloudapp.azure.com:27017");
            var database = mongoClient.GetDatabase("test");
            var messagesCollection = database.GetCollection<BsonDocument>("messages");

            var destinationContext = new EnronSqlContext();

            var emailConverter = new EmailConverter(new EmailAccountProvider(destinationContext));

            var emailTransferManager = new EmailTransferManager(
                messagesCollection,
                destinationContext,
                emailConverter);

            emailTransferManager.TransferAll();
        }
    }
}
