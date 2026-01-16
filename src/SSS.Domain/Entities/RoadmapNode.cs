using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class RoadmapNode
{
    public long Id { get; set; }
    public long RoadmapId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public string? Difficulty { get; set; }
    public int? OrderNo { get; set; }

    // Navigation
    public Roadmap Roadmap { get; set; } = null!;
    public ICollection<NodeContent> Contents { get; set; } = new HashSet<NodeContent>();
    public ICollection<RoadmapEdge> OutgoingEdges { get; set; } = new HashSet<RoadmapEdge>();
    public ICollection<RoadmapEdge> IncomingEdges { get; set; } = new HashSet<RoadmapEdge>();
    public ICollection<StudyPlanModule> StudyPlanModules { get; set; } = new HashSet<StudyPlanModule>();
    public ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}