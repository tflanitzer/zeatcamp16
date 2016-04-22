using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Driver;
using PerformanceTestUtil;

namespace StructuredMongoDB.Queries
{
    public abstract class QueryBase : IQuery
    {
        public IMongoCollection<BsonDocument> Collection { get; set; }

        public virtual BsonDocument Filter { get; } = new BsonDocument();
        public virtual PipelineDefinition<BsonDocument, BsonDocument> AggregatePipeline { get; } = null;
        public virtual MongoQueryType QueryType => MongoQueryType.Find;

        protected QueryBase(IMongoCollection<BsonDocument> collection)
        {
            Collection = collection;
        }

        public QueryResult Run()
        {
            if (QueryType == MongoQueryType.Find)
            {
                var results = Collection.Find(Filter);
                return new QueryResult { ResultsAsJsonString = results.ToJson(), ResultsCount = (int)results.Count() }; ;
            }
            else // if (QueryType == MongoQueryType.Aggregate)
            {
                var results = Collection.Aggregate(AggregatePipeline);
                var resultsCount = results.ToEnumerable().Count();
                return new QueryResult { ResultsAsJsonString = results.ToJson(), ResultsCount = resultsCount }; ;

            }
        }
    }
}