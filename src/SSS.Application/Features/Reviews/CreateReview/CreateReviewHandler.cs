using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.Reviews.Common;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.Reviews.CreateReview
{
    public class CreateReviewHandler(IAppDbContext context, IMapper mapper)
        : IRequestHandler<CreateReviewCommand, CreateReviewResult>
    {
        public async Task<CreateReviewResult> Handle(CreateReviewCommand req, CancellationToken ct)
        {
            // Kiểm tra roadmap có tồn tại không
            var roadmapExists = await context.Roadmaps
                .AnyAsync(r => r.Id == req.RoadmapId, ct);

            if (!roadmapExists)
                throw new NotFoundException($"Roadmap with Id {req.RoadmapId} not found.");

            // Chỉ user đã có StudyPlan (đã join) mới được review roadmap đó
            var hasStudyPlan = await context.StudyPlans
                .AnyAsync(sp => sp.RoadmapId == req.RoadmapId && sp.UserId == req.ReviewerId, ct);

            if (!hasStudyPlan)
                throw new ForbiddenException("You must join this roadmap before submitting a review.");

            // Mỗi user chỉ được review 1 lần trên 1 roadmap
            var alreadyReviewed = await context.Reviews
                .AnyAsync(r => r.RoadmapId == req.RoadmapId && r.ReviewerId == req.ReviewerId, ct);

            if (alreadyReviewed)
                throw new ConflictException("You have already submitted a review for this roadmap.");

            var review = new Review
            {
                RoadmapId = req.RoadmapId,
                ReviewerId = req.ReviewerId,
                Comment = req.Comment,
                Rating = req.Rating,
                CreatedAt = DateTime.UtcNow
            };

            context.Reviews.Add(review);
            await context.SaveChangesAsync(ct);

            var dto = await context.Reviews
                .AsNoTracking()
                .Where(r => r.Id == review.Id)
                .Include(r => r.Roadmap)
                .Include(r => r.Reviewer)
                .ProjectTo<ReviewDto>(mapper.ConfigurationProvider)
                .FirstAsync(ct);

            return new CreateReviewResult
            {
                Success = true,
                Message = "Review created successfully.",
                Data = dto
            };
        }
    }
}
