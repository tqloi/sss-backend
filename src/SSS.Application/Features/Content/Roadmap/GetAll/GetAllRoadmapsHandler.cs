using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.Roadmap.GetAll
{
    public sealed class GetAllRoadmapsHandler(IAppDbContext dbContext) 
        : IRequestHandler<GetAllRoadmapsQuery, GetAllRoadmapsResult>
    {
        public async Task<GetAllRoadmapsResult> Handle(GetAllRoadmapsQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.Roadmaps
                .Include(x => x.Subject)
                .AsNoTracking();

            // Public roadmap listing should only expose active records.
            query = query.Where(x => x.Status == RoadmapStatus.Active);

            // Filter by CategoryId if provided
            if (request.CategoryId.HasValue)
            {
                query = query.Where(x => dbContext.LearningSubjects
                    .AsNoTracking()
                    .Any(s => s.Id == x.SubjectId && s.CategoryId == request.CategoryId.Value));
            }

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

            // Order by Id descending
            query = query.OrderByDescending(x => x.Id);

            var paginated = await PaginatedResponse<Domain.Entities.Content.Roadmap>
                .CreateAsync(query, request.PageIndex, request.PageSize, cancellationToken);

            var result = paginated.MapItems(roadmap => new RoadmapListItemDTO
            {
                Id = roadmap.Id,
                SubjectId = roadmap.SubjectId,
                SubjectName = roadmap.Subject.Name,
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
