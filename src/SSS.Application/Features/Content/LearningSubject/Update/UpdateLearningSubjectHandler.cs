using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.LearningSubject.Update
{
    public sealed class UpdateLearningSubjectHandler(IAppDbContext dbContext) 
        : IRequestHandler<UpdateLearningSubjectCommand, UpdateLearningSubjectResult>
    {
        public async Task<UpdateLearningSubjectResult> Handle(UpdateLearningSubjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.LearningSubjects
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new UpdateLearningSubjectResult
                {
                    Success = false,
                    Message = "Learning subject not found.",
                    Data = null
                };
            }

            // Check if CategoryId exists
            var categoryExists = await dbContext.LearningCategories
                .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return new UpdateLearningSubjectResult
                {
                    Success = false,
                    Message = "Learning category not found.",
                    Data = null
                };
            }

            entity.CategoryId = request.CategoryId;
            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.IsActive = request.IsActive;

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateLearningSubjectResult
            {
                Success = true,
                Message = "Learning subject updated successfully.",
                Data = new LearningSubjectDTO
                {
                    Id = entity.Id,
                    CategoryId = entity.CategoryId,
                    Name = entity.Name,
                    Description = entity.Description,
                    IsActive = entity.IsActive
                }
            };
        }
    }
}
