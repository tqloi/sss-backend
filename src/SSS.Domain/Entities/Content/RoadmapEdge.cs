using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Content;

public class RoadmapEdge
{
    public long Id { get; set; }
    public long RoadmapId { get; set; }
    public long FromNodeId { get; set; }
    public long ToNodeId { get; set; }
    public EdgeType EdgeType { get; set; } = EdgeType.Prerequisite;
    public int? OrderNo { get; set; }

    // Navigation
    public virtual Roadmap Roadmap { get; set; } = null!;
    public virtual RoadmapNode FromNode { get; set; } = null!;
    public virtual RoadmapNode ToNode { get; set; } = null!;
}