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
            var activeAssignments = await dbContext.ContentManagerSubjects
                .Where(x => x.ContentManagerId == request.ContentManagerId && x.IsActive)
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
