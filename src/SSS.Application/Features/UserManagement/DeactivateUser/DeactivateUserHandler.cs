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

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
