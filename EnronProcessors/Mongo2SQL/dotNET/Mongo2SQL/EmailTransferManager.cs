using System.Security.Policy;
using Mongo2SQL.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Mongo2SQL
{
    public class EmailTransferManager
    {
        public IMongoCollection<BsonDocument> SourceCollection { get; private set; }
        public EnronSqlContext DestinationContext { get; private set; }
        public EmailConverter EmailConverter { get; private set; }

        public EmailTransferManager(
            IMongoCollection<BsonDocument> sourceCollection,
            EnronSqlContext destinationContext,
            EmailConverter emailConverter)
        {
            SourceCollection = sourceCollection;
            DestinationContext = destinationContext;
            EmailConverter = emailConverter;
        }

        public void TransferAll()
        {
            foreach (var bsonMessage in SourceCollection.AsQueryable())
            {
                var mail = EmailConverter.ConvertEmail(bsonMessage);

                DestinationContext.Mail.Add(mail);
//                DestinationContext.SaveChanges();
            }
        }
    }
}