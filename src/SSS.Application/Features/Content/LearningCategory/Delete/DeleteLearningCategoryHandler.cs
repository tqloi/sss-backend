using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.LearningCategory.Delete
{
    public sealed class DeleteLearningCategoryHandler(IAppDbContext dbContext) 
        : IRequestHandler<DeleteLearningCategoryCommand, DeleteLearningCategoryResult>
    {
        public async Task<DeleteLearningCategoryResult> Handle(DeleteLearningCategoryCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.LearningCategories
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new DeleteLearningCategoryResult
                {
                    Success = false,
                    Message = "Learning category not found.",
                    Data = null
                };
            }

            dbContext.LearningCategories.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteLearningCategoryResult
            {
                Success = true,
                Message = "Learning category deleted successfully.",
                Data = null
            };
        }
    }
}
