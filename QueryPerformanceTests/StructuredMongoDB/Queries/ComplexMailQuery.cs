using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StructuredMongoDB.Queries
{
    public class ComplexMailQuery : QueryBase
    {
        public ComplexMailQuery(IMongoCollection<BsonDocument> collection) : base(collection) {}

        public override BsonDocument Filter => BsonDocument.Parse("{\"sender\": \"jason.bass2@compaq.com\", \"receipients.EMailAddress\":\"eric.bass@enron.com\", date: {$gte: ISODate(\"2000-11-01T00:00:00.000Z\"), $lt: ISODate(\"2000-11-30T00:00:00.000Z\")}, $or: [{ subject: /.*Ramiro.*/}, { content: /.*Ramiro.*/ }]}");


    }
}