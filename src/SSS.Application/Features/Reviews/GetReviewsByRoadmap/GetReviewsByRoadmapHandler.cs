using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
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
            // ProjectTo trực tiếp: EF tự sinh JOIN, CountAsync & Skip/Take hoạt động chuẩn
            var query = context.Reviews
                .Where(r => r.RoadmapId == req.RoadmapId)
                .OrderByDescending(r => r.CreatedAt)
                .ProjectTo<ReviewDto>(mapper.ConfigurationProvider);

            var result = await PaginatedResponse<ReviewDto>
                .CreateAsync(query, req.PageIndex, req.PageSize, ct);

            return new GetReviewsByRoadmapResult
            {
                Success = true,
                Data = result
            };
        }
    }
}
