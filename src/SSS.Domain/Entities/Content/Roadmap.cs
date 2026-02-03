using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Content;

public class Roadmap
{
    public long Id { get; set; }
    public long SubjectId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int Version { get; set; } = 1;
    public bool IsLatest { get; set; }
    public RoadmapStatus Status { get; set; } = RoadmapStatus.Active;
    public string? CreateById { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public virtual LearningSubject Subject { get; set; } = null!;
    public virtual User CreateBy { get; set; } = null!;

    public virtual ICollection<RoadmapNode> Nodes { get; set; } = new HashSet<RoadmapNode>();
    public virtual ICollection<RoadmapEdge> Edges { get; set; } = new HashSet<RoadmapEdge>();
    public virtual ICollection<StudyPlan> StudyPlans { get; set; } = new HashSet<StudyPlan>();
}