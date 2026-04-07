using MediatR;

namespace SSS.Application.Features.Content.Roadmap.Delete
{
    public sealed record DeleteRoadmapCommand(long RoadmapId) : IRequest<DeleteRoadmapResult>;
}
