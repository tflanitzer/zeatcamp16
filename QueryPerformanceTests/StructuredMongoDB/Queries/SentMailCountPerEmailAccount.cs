using System.Data.SqlClient;
using MongoDB.Bson;
using MongoDB.Driver;

namespace StructuredMongoDB.Queries
{
    public class SentMailCountPerEmailAccount : QueryBase
    {
        public SentMailCountPerEmailAccount(IMongoCollection<BsonDocument> collection) : base(collection)
        {
        }

        public override MongoQueryType QueryType => MongoQueryType.Aggregate;


        // db.fatMail.aggregate([{ $group: { _id: { sender : '$sender' }, count: {$sum: 1 }}}, { $sort: {"count": -1}}])
        public override PipelineDefinition<BsonDocument, BsonDocument> AggregatePipeline => new[]
        {
            BsonDocument.Parse("{ $group: { _id: { sender : '$sender' }, count: {$sum: 1 }}}"),
            BsonDocument.Parse("{ $sort: {\"count\": -1}}"), 

            // Alternative with fluent API
            //Collection.Aggregate()
            //.Group(
            //    new BsonDocument
            //    {
            //        {"_id", new BsonDocument {{"sender", "$sender"}}},
            //        {"count", new BsonDocument {{"count", new BsonDocument {{"$sum", 1}}}}}
            //    })
            //.Sort(
            //    new BsonDocument
            //    {
            //        {
            //            "$count", -1
            //        }
            //    });

            // Without fluent
            //new BsonDocument
            //{
            //    {
            //        "$group", new BsonDocument
            //        {
            //            {"_id", new BsonDocument {{"sender", "$sender"}}},
            //            {"count", new BsonDocument {{"count", new BsonDocument {{"$sum", 1}}}}}
            //        }
            //    }
            //},
            //new BsonDocument
            //{
            //    {
            //        "$sort", new BsonDocument
            //        {
            //            {
            //                "$count", -1
            //            }
            //        }
            //    }
            //}

        };
    }
}