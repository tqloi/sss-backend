using SSS.Domain.Entities.Tracking;

namespace SSS.Domain.Entities.Planning;

public class TaskItem
{
    public long Id { get; set; }
    public long StudyPlanModuleId { get; set; }
    public bool IsGenerateByAI { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public Domain.Enums.TaskStatus? Status { get; set; }
    public int EstimatedDurationSeconds { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string? ExpectOutput{ get; set; }

    // Navigation
    public virtual StudyPlanModule StudyPlanModule { get; set; } = null!;
    public virtual ICollection<SessionTask> SessionTasks { get; set; } = new HashSet<SessionTask>();
}