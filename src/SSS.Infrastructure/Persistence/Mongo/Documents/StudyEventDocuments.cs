using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Persistence.Mongo.Documents
{
    [BsonCollection("study_events")]
    public class StudyEventDocument : MongoDocument
    {
        public string SessionId { get; set; } = null!;

        [BsonRepresentation(BsonType.String)]
        public StudyEventType EventType { get; set; } = StudyEventType.Start;

        [BsonRepresentation(BsonType.String)]
        public StudyEventCategory EventCategory { get; set; } = StudyEventCategory.Learning;

        [BsonRepresentation(BsonType.String)]
        public ContentMode ContentMode { get; set; } = ContentMode.Text;

        public DateTime EventTimestamp { get; set; }

        public object Payload { get; set; } = null!;
        public object DeviceInfo { get; set; } = null!;
    }
}
