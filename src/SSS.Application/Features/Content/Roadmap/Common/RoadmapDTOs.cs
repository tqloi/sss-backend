using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.Roadmap.Common
{
    public sealed class RoadmapBasicDTO
    {
        public long Id { get; set; }
        public long SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }

    public sealed class RoadmapListItemDTO
    {
        public long Id { get; set; }
        public long SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public string? Status { get; set; } // Placeholder for future
    }

    public sealed class RoadmapNodeDTO
    {
        public long Id { get; set; }
        public long RoadmapId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public NodeDifficulty? Difficulty { get; set; }
        public int? OrderNo { get; set; }
    }

    public sealed class RoadmapEdgeDTO
    {
        public long Id { get; set; }
        public long RoadmapId { get; set; }
        public long FromNodeId { get; set; }
        public long ToNodeId { get; set; }
        public EdgeType EdgeType { get; set; }
        public int? OrderNo { get; set; }
    }

    public sealed class RoadmapGraphDTO
    {
        public RoadmapBasicDTO Roadmap { get; set; } = null!;
        public List<RoadmapNodeDTO> Nodes { get; set; } = new();
        public List<RoadmapEdgeDTO> Edges { get; set; } = new();
    }

    public sealed class NodeContentDTO
    {
        public long Id { get; set; }
        public long NodeId { get; set; }
        public ContentType ContentType { get; set; }
        public string Title { get; set; } = null!;
        public string? Url { get; set; }
        public string? Description { get; set; }
        public int? EstimatedMinutes { get; set; }
        public string? Difficulty { get; set; }
        public int OrderNo { get; set; }
        public bool IsRequired { get; set; }
    }
}
