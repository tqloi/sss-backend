using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("posthog_events")]
    public class PostHogEventDocument : MongoDocument
    {        

        [BsonElement("properties")]
        public BsonDocument? Properties { get; set; }

        [BsonElement("raw_payload")]
        public BsonDocument? RawPayload { get; set; }  

        [BsonElement("receivedAt")]
        public DateTime ReceivedAt { get; set; }
    }
}
