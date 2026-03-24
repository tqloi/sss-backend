using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.UserManagement.DeactivateUser
{
    public sealed class DeactivateUserHandler(IAppDbContext dbContext)
        : IRequestHandler<DeactivateUserCommand, bool>
    {
        public async Task<bool> Handle(DeactivateUserCommand request, CancellationToken ct)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (user is null)
            {
                return false;
            }

            user.IsActive = false;
            user.LockoutEnabled = true;
            user.LockoutEnd = DateTimeOffset.UtcNow.AddYears(100);

            var now = DateTime.UtcNow;
            var activeRefreshTokens = await dbContext.RefreshTokens
                .Where(x => x.UserId == user.Id && x.RevokedAtUtc == null)
                .ToListAsync(ct);

            foreach (var token in activeRefreshTokens)
            {
                token.IsUsed = true;
                token.RevokedAtUtc = now;
                token.RevokedByIp = "admin-block";
            }

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
