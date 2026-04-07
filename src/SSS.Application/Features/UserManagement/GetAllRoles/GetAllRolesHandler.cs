using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.UserManagement.GetAllRoles
{
    public sealed class GetAllRolesHandler(IAppDbContext dbContext)
        : IRequestHandler<GetAllRolesQuery, GetAllRolesResponse>
    {
        public async Task<GetAllRolesResponse> Handle(GetAllRolesQuery request, CancellationToken ct)
        {
            var roles = await dbContext.Roles
                .AsNoTracking()
                .Where(r => r.Name != null)
                .Select(r => r.Name!)
                .Distinct()
                .OrderBy(r => r)
                .ToListAsync(ct);

            return new GetAllRolesResponse
            {
                Roles = roles
            };
        }
    }
}
