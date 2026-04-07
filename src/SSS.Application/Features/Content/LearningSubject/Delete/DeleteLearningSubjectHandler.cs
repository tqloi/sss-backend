using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.LearningSubject.Delete
{
    public sealed class DeleteLearningSubjectHandler(IAppDbContext dbContext) 
        : IRequestHandler<DeleteLearningSubjectCommand, DeleteLearningSubjectResult>
    {
        public async Task<DeleteLearningSubjectResult> Handle(DeleteLearningSubjectCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.LearningSubjects
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new DeleteLearningSubjectResult
                {
                    Success = false,
                    Message = "Learning subject not found.",
                    Data = null
                };
            }

            dbContext.LearningSubjects.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteLearningSubjectResult
            {
                Success = true,
                Message = "Learning subject deleted successfully.",
                Data = null
            };
        }
    }
}
