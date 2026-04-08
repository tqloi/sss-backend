using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.UserManagement.AssignSubjectToContentManager
{
    public sealed class AssignSubjectToContentManagerHandler(IAppDbContext dbContext)
        : IRequestHandler<AssignSubjectToContentManagerCommand, bool>
    {
        public async Task<bool> Handle(AssignSubjectToContentManagerCommand request, CancellationToken ct)
        {
            var currentAssignment = await dbContext.ContentManagerSubjects
                .FirstOrDefaultAsync(
                    x => x.ContentManagerId == request.ContentManagerId && x.SubjectId == request.SubjectId,
                    ct);

            if (currentAssignment is null)
            {
                currentAssignment = new ContentManagerSubject
                {
                    ContentManagerId = request.ContentManagerId,
                    SubjectId = request.SubjectId,
                    AssignedBy = request.AssignedBy,
                    AssignedAt = DateTime.UtcNow,
                    IsActive = true,
                };

                dbContext.ContentManagerSubjects.Add(currentAssignment);
            }
            else
            {
                currentAssignment.IsActive = true;
                currentAssignment.AssignedBy = request.AssignedBy;
                currentAssignment.AssignedAt = DateTime.UtcNow;
            }

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
