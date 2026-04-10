using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.Common
{
    public class ActiveSessionCacheDto
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public SessionStatus Status { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? PausedAt { get; set; }
        public int PauseCount { get; set; }
        public int PauseSeconds { get; set; }
        public long? StudyPlanId { get; set; }
        public long? StudyPlanModuleId { get; set; }
        public int? PlannedDurationSeconds { get; set; }
        public string? Timezone { get; set; }
        
        public List<ActiveSessionTaskCacheDto> Tasks { get; set; } = new();
    }

    public class ActiveSessionTaskCacheDto
    {
        public long Id { get; set; }
        public long TaskId { get; set; }
        public string Status { get; set; } = "INCOMPLETE";
        public DateTime? StartTimeUtc { get; set; }
        public DateTime? EndTimeUtc { get; set; }
    }
}
