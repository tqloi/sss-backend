using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.UserGamifications.DeleteUserGamification;

public sealed class DeleteUserGamificationHandler(
    IAppDbContext dbContext
) : IRequestHandler<DeleteUserGamificationCommand, bool>
{
    public async Task<bool> Handle(DeleteUserGamificationCommand request, CancellationToken ct)
    {
        var gamification = await dbContext.UserGamifications
            .FirstOrDefaultAsync(g => g.UserId == request.UserId, ct);

        if (gamification is null)
            return false;

        dbContext.UserGamifications.Remove(gamification);
        await dbContext.SaveChangesAsync(ct);

        return true;
    }
}
