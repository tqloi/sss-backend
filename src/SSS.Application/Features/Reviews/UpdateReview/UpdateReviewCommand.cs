using MediatR;

namespace SSS.Application.Features.Reviews.UpdateReview
{
    public sealed record UpdateReviewCommand : IRequest<UpdateReviewResult>
    {
        public long Id { get; init; }
        public string RequesterId { get; init; } = null!;
        public string? Comment { get; init; }
        public int Rating { get; init; }
    }
}
