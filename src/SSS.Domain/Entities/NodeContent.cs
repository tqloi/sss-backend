namespace SSS.Domain.Entities;

public class NodeContent
{
    public long Id { get; set; }
    public long NodeId { get; set; }
    public string ContentType { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Url { get; set; }
    public string? Description { get; set; }
    public int? EstimatedMinutes { get; set; }
    public string? Difficulty { get; set; }
    public int OrderNo { get; set; }
    public bool IsRequired { get; set; }

    // Navigation
    public RoadmapNode Node { get; set; } = null!;
}