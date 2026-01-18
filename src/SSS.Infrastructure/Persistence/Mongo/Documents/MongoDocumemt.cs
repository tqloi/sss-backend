using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    public abstract class MongoDocument
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; } = null!;
    }
}
