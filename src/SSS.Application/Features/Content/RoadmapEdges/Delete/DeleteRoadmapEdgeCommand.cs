using MediatR;

namespace SSS.Application.Features.Content.RoadmapEdges.Delete
{
    public sealed record DeleteRoadmapEdgeCommand(long RoadmapId, long EdgeId) : IRequest<DeleteRoadmapEdgeResult>;
}
