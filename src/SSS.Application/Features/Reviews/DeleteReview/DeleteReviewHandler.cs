using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;

namespace SSS.Application.Features.Reviews.DeleteReview
{
    public class DeleteReviewHandler(IAppDbContext context)
        : IRequestHandler<DeleteReviewCommand, DeleteReviewResult>
    {
        public async Task<DeleteReviewResult> Handle(DeleteReviewCommand req, CancellationToken ct)
        {
            var review = await context.Reviews
                .FirstOrDefaultAsync(r => r.Id == req.Id, ct);

            if (review == null)
                throw new NotFoundException($"Review with Id {req.Id} not found.");

            // Chỉ chủ review hoặc admin mới được xóa
            if (!req.IsAdmin && review.ReviewerId != req.RequesterId)
                throw new ForbiddenException("You are not allowed to delete this review.");

            context.Reviews.Remove(review);
            await context.SaveChangesAsync(ct);

            return new DeleteReviewResult(IsDeleted: true, Message: "Review deleted successfully.");
        }
    }
}
