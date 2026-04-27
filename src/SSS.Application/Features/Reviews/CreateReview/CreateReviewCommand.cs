using MediatR;

namespace SSS.Application.Features.Reviews.CreateReview
{
    public sealed record CreateReviewCommand : IRequest<CreateReviewResult>
    {
        public long RoadmapId { get; init; }
        public string ReviewerId { get; init; } = null!;
        public string? Comment { get; init; }
        public int Rating { get; init; }
    }
}
