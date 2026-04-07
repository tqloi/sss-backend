using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.LearningSubject.GetAll
{
    public sealed class GetAllLearningSubjectHandler(IAppDbContext dbContext) 
        : IRequestHandler<GetAllLearningSubjectQuery, GetAllLearningSubjectResult>
    {
        public async Task<GetAllLearningSubjectResult> Handle(GetAllLearningSubjectQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.LearningSubjects.AsNoTracking();

            // Filter by CategoryId if provided
            if (request.CategoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == request.CategoryId.Value);
            }

            query = query.OrderBy(x => x.Id);

            var paginated = await PaginatedResponse<Domain.Entities.Content.LearningSubject>
                .CreateAsync(query, request.PageIndex, request.PageSize, cancellationToken);

            var result = paginated.MapItems(subject => new LearningSubjectDTO
            {
                Id = subject.Id,
                CategoryId = subject.CategoryId,
                Name = subject.Name,
                Description = subject.Description,
                IsActive = subject.IsActive
            });

            return new GetAllLearningSubjectResult(result);
        }
    }
}
