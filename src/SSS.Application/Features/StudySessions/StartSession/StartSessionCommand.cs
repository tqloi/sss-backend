using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudySessions.StartSession
{
    public class StartSessionCommand : IRequest<StartSessionResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public long? StudyPlanId { get; set; }
        public long? NodeId { get; set; }
        public long? ModuleId { get; set; }        
        public long[]? TaskIds { get; set; }
        public int? PlannedDurationSeconds { get; set; }
        public string? Timezone { get; set; }
    }
}
