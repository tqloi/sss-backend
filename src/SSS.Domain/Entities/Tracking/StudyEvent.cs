using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Tracking
{
    public class StudyEvent
    {
        public string Id { get; set; } = null!;
        public string SessionId { get; set; } = null!;
        public StudyEventType EventType { get; set; } = StudyEventType.Start;
        public StudyEventCategory EventCategory { get; set; } = StudyEventCategory.Learning;
        public ContentMode ContentMode { get; set; } = ContentMode.Text;
        public DateTime EventTimestamp { get; set; }
        public object Payload { get; set; } = null!;
        public object DeviceInfo { get; set; } = null!;
    }
}
