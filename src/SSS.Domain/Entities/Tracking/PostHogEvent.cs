using MongoDB.Bson;
using System.Text.Json;

namespace SSS.Domain.Entities.Tracking
{
    public class PostHogEvent
    {        
        public BsonDocument? Properties { get; set; }  
        public BsonDocument? RawPayload { get; set; }
        public DateTime ReceivedAt { get; set; }
    }
}
