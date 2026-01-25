using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.LearningCategory.Common;

namespace SSS.Application.Features.Content.LearningCategory.GetAll
{
    public sealed class GetAllLearningCategoryHandler(IAppDbContext dbContext) 
        : IRequestHandler<GetAllLearningCategoryQuery, GetAllLearningCategoryResult>
    {
        public async Task<GetAllLearningCategoryResult> Handle(GetAllLearningCategoryQuery request, CancellationToken cancellationToken)
        {
            var query = dbContext.LearningCategories
                .AsNoTracking()
                .OrderBy(x => x.Id);

            var paginated = await PaginatedResponse<Domain.Entities.Content.LearningCategory>
                .CreateAsync(query, request.PageIndex, request.PageSize, cancellationToken);

            var result = paginated.MapItems(category => new LearningCategoryDTO
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive
            });

            return new GetAllLearningCategoryResult(result);
        }
    }
}
