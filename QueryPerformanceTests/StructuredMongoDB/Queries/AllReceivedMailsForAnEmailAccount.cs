using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StructuredMongoDB.Queries
{
    public class AllReceivedMailsForAnEmailAccount : QueryBase
    {
        public AllReceivedMailsForAnEmailAccount(IMongoCollection<BsonDocument> collection) : base(collection) {}

        public override BsonDocument Filter => BsonDocument.Parse("{\"receipients.EMailAddress\":\"eric.bass@enron.com\"}");

    }
}