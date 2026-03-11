using MediatR;
using System.Text.Json.Serialization;

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
        public int? ActiveSeconds { get; set; }
        public int? IdleSeconds { get; set; }
        public long[]? TasksCompleted { get; set; }
        public int? FocusScore { get; set; }
        public int? FatigueScore { get; set; }
    }
}
