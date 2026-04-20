using MediatR;

namespace SSS.Application.Features.Reviews.DeleteReview
{
    public sealed record DeleteReviewCommand : IRequest<DeleteReviewResult>
    {
        public long Id { get; init; }
        public string RequesterId { get; init; } = null!;
        public bool IsAdmin { get; init; }
    }
}
