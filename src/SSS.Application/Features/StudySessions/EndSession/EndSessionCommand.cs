using MediatR;
using System.Text.Json.Serialization;
using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.StudySessions.EndSession
{
    public class EndSessionCommand : IRequest<EndSessionResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        [JsonIgnore]
        public string SessionId { get; set; } = null!;
        public string? EndedReason { get; set; }
        public int? SelfRating { get; set; }
        public string? Notes { get; set; }
        public int? ActualDurationSeconds { get; set; }
        public List<EndSessionTaskDto>? Tasks { get; set; }
    }
}
