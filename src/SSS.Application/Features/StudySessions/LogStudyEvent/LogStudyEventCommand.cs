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
        public string EventType { get; set; } = null!;
        public long? TaskId { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }
}
