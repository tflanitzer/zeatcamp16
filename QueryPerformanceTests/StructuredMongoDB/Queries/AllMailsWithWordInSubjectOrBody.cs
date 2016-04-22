using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StructuredMongoDB.Queries
{
    public class AllMailsWithWordInSubjectOrBody : QueryBase
    {
        public AllMailsWithWordInSubjectOrBody(IMongoCollection<BsonDocument> collection) : base(collection) {}

        public override BsonDocument Filter => BsonDocument.Parse("{ $or: [{\"subject\":/.million.*/},{\"content\":/.million.*/}]}");
    }
}