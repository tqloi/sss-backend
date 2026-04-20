using MediatR;

namespace SSS.Application.Features.Reviews.GetReviewsByRoadmap
{
    public sealed record GetReviewsByRoadmapQuery : IRequest<GetReviewsByRoadmapResult>
    {
        public long RoadmapId { get; init; }
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
