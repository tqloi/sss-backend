using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.Roadmap.GetAll
{
    public sealed class GetAllRoadmapsHandler(IAppDbContext dbContext) 
        : IRequestHandler<GetAllRoadmapsQuery, GetAllRoadmapsResult>
    {
        public async Task<GetAllRoadmapsResult> Handle(GetAllRoadmapsQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Roadmaps.AsNoTracking();

            // Filter by SubjectId if provided
            if (request.SubjectId.HasValue)
            {
                query = query.Where(x => x.SubjectId == request.SubjectId.Value);
            }

            // Search by Title if q provided
            if (!string.IsNullOrWhiteSpace(request.Q))
            {
                query = query.Where(x => x.Title.Contains(request.Q));
            }

            // Filter by Version if provided
            if (request.Version.HasValue)
            {
                query = query.Where(x => x.Version == request.Version.Value);
            }

            // Filter by IsLatest if provided
            if (request.IsLatest.HasValue)
            {
                query = query.Where(x => x.IsLatest == request.IsLatest.Value);
            }

            // Status filter - placeholder, no actual field exists yet
            // If Status field is added in future, implement here

            // Order by Id descending
            query = query.OrderByDescending(x => x.Id);

            var paginated = await PaginatedResponse<Domain.Entities.Content.Roadmap>
                .CreateAsync(query, request.PageIndex, request.PageSize, cancellationToken);

            var result = paginated.MapItems(roadmap => new RoadmapListItemDTO
            {
                Id = roadmap.Id,
                SubjectId = roadmap.SubjectId,
                Title = roadmap.Title,
                Description = roadmap.Description,
                Version = roadmap.Version,
                IsLatest = roadmap.IsLatest,
                CreatedById = roadmap.CreateById,
                CreatedAt = roadmap.CreatedAt,
                Status = roadmap.Status 
            });

            return new GetAllRoadmapsResult(result);
        }
    }
}
