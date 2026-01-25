using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.Roadmaps.Common;

// Request DTOs
public record RoadmapGraphCreateRequest
{
    public required RoadmapGraphMetadata Roadmap { get; init; }
    public List<RoadmapNodeGraphItem> Nodes { get; init; } = new();
    public List<NodeContentGraphItem> Contents { get; init; } = new();
    public List<RoadmapEdgeGraphItem> Edges { get; init; } = new();
}

public record RoadmapGraphUpdateRequest
{
    public RoadmapGraphUpdateMetadata? Roadmap { get; init; }
    public List<RoadmapNodeGraphItem> Nodes { get; init; } = new();
    public List<NodeContentGraphItem> Contents { get; init; } = new();
    public List<RoadmapEdgeGraphItem> Edges { get; init; } = new();
}

public record RoadmapGraphMetadata
{
    public required long SubjectId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
}

public record RoadmapGraphUpdateMetadata
{
    public string? Title { get; init; }
    public string? Description { get; init; }
}

public record RoadmapNodeGraphItem
{
    public long? Id { get; init; }
    public string? ClientId { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public NodeDifficulty? Difficulty { get; init; }
    public int? OrderNo { get; init; }
}

public record NodeContentGraphItem
{
    public long? Id { get; init; }
    public string? ClientId { get; init; }
    public long? NodeId { get; init; }
    public string? NodeClientId { get; init; }
    public required ContentType ContentType { get; init; }
    public required string Title { get; init; }
    public string? Url { get; init; }
    public string? Description { get; init; }
    public int? EstimatedMinutes { get; init; }
    public string? Difficulty { get; init; }
    public required int OrderNo { get; init; }
    public required bool IsRequired { get; init; }
}

public record RoadmapEdgeGraphItem
{
    public long? Id { get; init; }
    public long? FromNodeId { get; init; }
    public string? FromNodeClientId { get; init; }
    public long? ToNodeId { get; init; }
    public string? ToNodeClientId { get; init; }
    public required EdgeType EdgeType { get; init; }
    public int? OrderNo { get; init; }
}

// Response DTOs
public record RoadmapGraphCreateResponse
{
    public required long RoadmapId { get; init; }
    public Dictionary<string, long> NodeIdMap { get; init; } = new();
    public Dictionary<string, long> ContentIdMap { get; init; } = new();
    public required GraphSummary Summary { get; init; }
}

public record RoadmapGraphUpdateResponse
{
    public required long RoadmapId { get; init; }
    public Dictionary<string, long> NodeIdMap { get; init; } = new();
    public Dictionary<string, long> ContentIdMap { get; init; } = new();
    public required GraphSummary Summary { get; init; }
}

public record GraphSummary
{
    public required int NodesCount { get; init; }
    public required int EdgesCount { get; init; }
    public required int ContentsCount { get; init; }
}
