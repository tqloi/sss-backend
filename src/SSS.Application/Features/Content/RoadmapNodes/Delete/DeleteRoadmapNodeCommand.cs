using MediatR;

namespace SSS.Application.Features.Content.RoadmapNodes.Delete
{
    public sealed record DeleteRoadmapNodeCommand(long RoadmapId, long NodeId) : IRequest<DeleteRoadmapNodeResult>;
}
