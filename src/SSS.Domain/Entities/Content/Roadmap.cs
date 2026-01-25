using SSS.Domain.Entities.Planning;

namespace SSS.Domain.Entities.Content;

public class Roadmap
{
    public long Id { get; set; }
    public long SubjectId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    // Navigation
    public virtual LearningSubject Subject { get; set; } = null!;
    public virtual ICollection<RoadmapNode> Nodes { get; set; } = new HashSet<RoadmapNode>();
    public virtual ICollection<RoadmapEdge> Edges { get; set; } = new HashSet<RoadmapEdge>();
    public virtual ICollection<StudyPlan> StudyPlans { get; set; } = new HashSet<StudyPlan>();
}