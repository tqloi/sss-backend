using MediatR;
using SSS.Application.Features.Content.Roadmaps.Common;

namespace SSS.Application.Features.Content.Roadmaps.GraphSync;

public sealed class SyncRoadmapGraphCommand : IRequest<SyncRoadmapGraphResult>
{
    public required long RoadmapId { get; set; }
    public RoadmapGraphUpdateMetadata? Roadmap { get; set; }
    public List<RoadmapNodeGraphItem> Nodes { get; set; } = new();
    public List<NodeContentGraphItem> Contents { get; set; } = new();
    public List<RoadmapEdgeGraphItem> Edges { get; set; } = new();
}
