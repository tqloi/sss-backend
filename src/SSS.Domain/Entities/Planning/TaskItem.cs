using SSS.Domain.Entities.Tracking;

namespace SSS.Domain.Entities.Planning;

public class TaskItem
{
    public long Id { get; set; }
    public long StudyPlanModuleId { get; set; }
    public string Title { get; set; } = null!;
    public TaskStatus? Status { get; set; }
    public int EstimatedDurationSeconds { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation
    public virtual StudyPlanModule StudyPlanModule { get; set; } = null!;
    public virtual ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}