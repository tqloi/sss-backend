using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Reviews.Common;

namespace SSS.Application.Features.Reviews.UpdateReview
{
    public class UpdateReviewHandler(IAppDbContext context, IMapper mapper)
        : IRequestHandler<UpdateReviewCommand, UpdateReviewResult>
    {
        public async Task<UpdateReviewResult> Handle(UpdateReviewCommand req, CancellationToken ct)
        {
            var review = await context.Reviews
                .FirstOrDefaultAsync(r => r.Id == req.Id, ct);

            if (review == null)
                throw new NotFoundException($"Review with Id {req.Id} not found.");

            // Chỉ chủ review mới được sửa
            if (review.ReviewerId != req.RequesterId)
                throw new ForbiddenException("You are not allowed to update this review.");

            review.Comment = req.Comment;
            review.Rating = req.Rating;
            review.UpdatedAt = DateTime.UtcNow;

            context.Reviews.Update(review);
            await context.SaveChangesAsync(ct);

            var dto = await context.Reviews
                .AsNoTracking()
                .Where(r => r.Id == review.Id)
                .Include(r => r.Roadmap)
                .Include(r => r.Reviewer)
                .ProjectTo<ReviewDto>(mapper.ConfigurationProvider)
                .FirstAsync(ct);

            return new UpdateReviewResult
            {
                Success = true,
                Message = "Review updated successfully.",
                Data = dto
            };
        }
    }
}
