using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.LearningSubject.Create
{
    public sealed class CreateLearningSubjectHandler(IAppDbContext dbContext) 
        : IRequestHandler<CreateLearningSubjectCommand, CreateLearningSubjectResult>
    {
        public async Task<CreateLearningSubjectResult> Handle(CreateLearningSubjectCommand request, CancellationToken cancellationToken)
        {
            // Check if CategoryId exists
            var categoryExists = await dbContext.LearningCategories
                .AnyAsync(x => x.Id == request.CategoryId, cancellationToken);

            if (!categoryExists)
            {
                return new CreateLearningSubjectResult
                {
                    Success = false,
                    Message = "Learning category not found.",
                    Data = null
                };
            }

            var entity = new Domain.Entities.Content.LearningSubject
            {
                CategoryId = request.CategoryId,
                Name = request.Name,
                Description = request.Description,
                IsActive = request.IsActive
            };

            dbContext.LearningSubjects.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateLearningSubjectResult
            {
                Success = true,
                Message = "Learning subject created successfully.",
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
