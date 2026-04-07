using MediatR;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.LearningCategory.Create
{
    using SSS.Application.Features.Content.LearningCategory.Common;
    using SSS.Domain.Entities.Content;
    public sealed class CreateLearningCategoryHandler(IAppDbContext dbContext) : IRequestHandler<CreateLearningCategoryCommand, CreateLearningCategoryResult>
    {
        public async Task<CreateLearningCategoryResult> Handle(CreateLearningCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = new LearningCategory
            {
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };
            dbContext.LearningCategories.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);
            return new CreateLearningCategoryResult
            {
                Success = true,
                Message = "Learning category created successfully.",
                Data = new LearningCategoryDTO
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                }
            };
        }
    }
}
