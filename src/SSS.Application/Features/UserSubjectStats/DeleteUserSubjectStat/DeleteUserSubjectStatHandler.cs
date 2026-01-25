using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.UserSubjectStats.DeleteUserSubjectStat;

public sealed class DeleteUserSubjectStatHandler(
    IAppDbContext dbContext
) : IRequestHandler<DeleteUserSubjectStatCommand, bool>
{
    public async Task<bool> Handle(DeleteUserSubjectStatCommand request, CancellationToken ct)
    {
        var stat = await dbContext.UserSubjectStats
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId, ct);

        if (stat is null)
            return false;

        dbContext.UserSubjectStats.Remove(stat);
        await dbContext.SaveChangesAsync(ct);

        return true;
    }
}
