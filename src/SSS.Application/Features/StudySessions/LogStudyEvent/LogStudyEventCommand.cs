using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.LogStudyEvent
{
    public class LogStudyEventCommand : IRequest<LogStudyEventResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public string SessionId { get; set; } = null!;

        /// <summary>
        /// Loại sự kiện: View, Click, Start, Submit, Complete
        /// </summary>
        public string EventType { get; set; } = null!;

        /// <summary>
        /// Danh mục: Learning, Assessment, Navigation, System (mặc định: Learning)
        /// </summary>
        public string? EventCategory { get; set; }

        /// <summary>
        /// Loại nội dung: Video, Text, Quiz, Practice (mặc định: Text)
        /// </summary>
        public string? ContentMode { get; set; }

        public long? TaskId { get; set; }
        public string? StudyPlanModuleId { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
