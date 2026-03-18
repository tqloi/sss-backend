using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Entities.Content;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Tracking;

public class StudySession
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;

    public SessionStatus Status { get; set; } = SessionStatus.NotStarted;
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public DateTime? PausedAt { get; set; }
    public SessionEndedReason? EndedReason { get; set; }
    public int? TasksCompletedCount { get; set; }
    public int? TotalTasks { get; set; }
    public int? PlannedDurationSeconds { get; set; }
    public int? ActualDurationSeconds { get; set; }
    public int? ActiveSeconds { get; set; }
    public int? IdleSeconds { get; set; }
    public int PauseCount { get; set; }
    public int PauseSeconds { get; set; }
    public int? FocusScore { get; set; }
    public decimal? ConfidenceActiveLearning { get; set; }
    public int? FatigueScore { get; set; }
    public int? SelfRating { get; set; }
    public LocalTimeBlock? LocalTimeBlock { get; set; }
    public string? Timezone { get; set; }
    public DateTime? CreatedAt { get; set; } = DateTime.MinValue;

    public long? StudyPlanId { get; set; }
    public long? StudyPlanModuleId { get; set; }

    // Navigation
    public virtual User User { get; set; } = null!;
    public virtual ICollection<SessionTask> SessionTasks { get; set; } = new List<SessionTask>();
}