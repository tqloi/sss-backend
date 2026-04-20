using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Reviews.Common;

namespace SSS.Application.Features.Reviews.GetReviewsByRoadmap
{
    public class GetReviewsByRoadmapHandler(IAppDbContext context, IMapper mapper)
        : IRequestHandler<GetReviewsByRoadmapQuery, GetReviewsByRoadmapResult>
    {
        public async Task<GetReviewsByRoadmapResult> Handle(GetReviewsByRoadmapQuery req, CancellationToken ct)
        {
            var query = context.Reviews
                .AsNoTracking()
                .Where(r => r.RoadmapId == req.RoadmapId)
                .Include(r => r.Roadmap)
                .Include(r => r.Reviewer)
                .OrderByDescending(r => r.CreatedAt);

            var paginated = await PaginatedResponse<Domain.Entities.Content.Review>
                .CreateAsync(query, req.PageIndex, req.PageSize, ct);

            var result = paginated.MapItems(r => mapper.Map<ReviewDto>(r));

            return new GetReviewsByRoadmapResult
            {
                Success = true,
                Data = result
            };
        }
    }
}
