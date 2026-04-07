using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.UserManagement.UnassignSubjectFromContentManager
{
    public sealed class UnassignSubjectFromContentManagerHandler(IAppDbContext dbContext)
        : IRequestHandler<UnassignSubjectFromContentManagerCommand, bool>
    {
        public async Task<bool> Handle(UnassignSubjectFromContentManagerCommand request, CancellationToken ct)
        {
            var activeAssignmentsQuery = dbContext.ContentManagerSubjects
                .Where(x => x.ContentManagerId == request.ContentManagerId && x.IsActive);

            if (request.SubjectId.HasValue)
            {
                activeAssignmentsQuery = activeAssignmentsQuery
                    .Where(x => x.SubjectId == request.SubjectId.Value);
            }

            var activeAssignments = await activeAssignmentsQuery
                .ToListAsync(ct);

            if (activeAssignments.Count == 0)
            {
                return true;
            }

            foreach (var assignment in activeAssignments)
            {
                assignment.IsActive = false;
            }

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
