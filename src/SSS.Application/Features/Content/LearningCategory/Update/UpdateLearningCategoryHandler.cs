using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.LearningCategory.Common;

namespace SSS.Application.Features.Content.LearningCategory.Update
{
    public sealed class UpdateLearningCategoryHandler(IAppDbContext dbContext) 
        : IRequestHandler<UpdateLearningCategoryCommand, UpdateLearningCategoryResult>
    {
        public async Task<UpdateLearningCategoryResult> Handle(UpdateLearningCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.LearningCategories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new UpdateLearningCategoryResult
                {
                    Success = false,
                    Message = "Learning category not found.",
                    Data = null
                };
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsActive = request.IsActive;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateLearningCategoryResult
            {
                Success = true,
                Message = "Learning category updated successfully.",
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
