using SSS.Domain.Entities.Content;
using SSS.Domain.Entities.Tracking;
using SSS.Domain.Enums;
using SSS.Domain.Entities.Assessment;

namespace SSS.Domain.Entities.Planning;

public class StudyPlanModule
{
    public long Id { get; set; }
    public long StudyPlanId { get; set; }
    public long RoadmapNodeId { get; set; }
    public ModuleStatus? Status { get; set; }

    // Navigation
    public virtual StudyPlan StudyPlan { get; set; } = null!;
    public virtual RoadmapNode RoadmapNode { get; set; } = null!;
    public virtual ICollection<TaskItem> Tasks { get; set; } = new HashSet<TaskItem>();
    public virtual ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}