using MediatR;
using SSS.Application.Features.Content.Roadmaps.Common;

namespace SSS.Application.Features.Content.Roadmaps.GraphCreate;

public sealed class CreateRoadmapGraphCommand : IRequest<CreateRoadmapGraphResult>
{
    public required RoadmapGraphMetadata Roadmap { get; set; }
    public List<RoadmapNodeGraphItem> Nodes { get; set; } = new();
    public List<NodeContentGraphItem> Contents { get; set; } = new();
    public List<RoadmapEdgeGraphItem> Edges { get; set; } = new();
}
