using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StructuredMongoDB.Queries
{
    public class MailsFilteredAndSortedByDate : QueryBase
    {
        public MailsFilteredAndSortedByDate(IMongoCollection<BsonDocument> collection) : base(collection) {}

        public override BsonDocument Filter => BsonDocument.Parse("{date: {$gte: ISODate(\"2000-11-01T00:00:00.000Z\"), $lt: ISODate(\"2000-11-10T00:00:00.000Z\")}}).sort({date:1}");

    }
}