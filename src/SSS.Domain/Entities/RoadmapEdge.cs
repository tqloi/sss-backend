namespace SSS.Domain.Entities;

public class RoadmapEdge
{
    public long Id { get; set; }
    public long RoadmapId { get; set; }
    public long FromNodeId { get; set; }
    public long ToNodeId { get; set; }
    public string EdgeType { get; set; } = null!;
    public int? OrderNo { get; set; }

    // Navigation
    public Roadmap Roadmap { get; set; } = null!;
    public RoadmapNode FromNode { get; set; } = null!;
    public RoadmapNode ToNode { get; set; } = null!;
}