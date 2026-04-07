using MediatR;

namespace SSS.Application.Features.Content.Roadmap.GetById
{
    public sealed record GetRoadmapByIdQuery(long RoadmapId) : IRequest<GetRoadmapByIdResult>;
}
