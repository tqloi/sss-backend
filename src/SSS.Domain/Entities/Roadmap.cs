using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class Roadmap
{
    public long Id { get; set; }
    public long SubjectId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    // Navigation
    public LearningSubject Subject { get; set; } = null!;
    public ICollection<RoadmapNode> Nodes { get; set; } = new HashSet<RoadmapNode>();
    public ICollection<RoadmapEdge> Edges { get; set; } = new HashSet<RoadmapEdge>();
    public ICollection<StudyPlan> StudyPlans { get; set; } = new HashSet<StudyPlan>();
}