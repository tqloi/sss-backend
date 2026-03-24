using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.UserManagement.ActivateUser
{
    public sealed class ActivateUserHandler(IAppDbContext dbContext)
        : IRequestHandler<ActivateUserCommand, bool>
    {
        public async Task<bool> Handle(ActivateUserCommand request, CancellationToken ct)
        {
            var user = await dbContext.Users
                .FirstOrDefaultAsync(x => x.Id == request.Id, ct);

            if (user is null)
            {
                return false;
            }

            user.IsActive = true;
            user.LockoutEnd = null;

            await dbContext.SaveChangesAsync(ct);
            return true;
        }
    }
}
