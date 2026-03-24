using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Dtos;
using SSS.Application.Features.UserManagement.Common;

namespace SSS.Application.Features.UserManagement.GetAllUsers
{
    public sealed class GetAllUsersHandler(
        IAppDbContext dbContext
    ) : IRequestHandler<GetAllUsersQuery, GetAllUsersResponse>
    {
        public async Task<GetAllUsersResponse> Handle(GetAllUsersQuery request, CancellationToken ct)
        {
            var usersQuery = dbContext.Users
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var nameFilter = request.Name.Trim().ToLower();
                usersQuery = usersQuery.Where(u =>
                    (u.UserName != null && u.UserName.ToLower().Contains(nameFilter)) ||
                    (u.FirstName != null && u.FirstName.ToLower().Contains(nameFilter)) ||
                    (u.LastName != null && u.LastName.ToLower().Contains(nameFilter)) ||
                    (u.Email != null && u.Email.ToLower().Contains(nameFilter))
                );
            }

            if (!string.IsNullOrWhiteSpace(request.Role))
            {
                var roleFilter = request.Role.Trim().ToLower();
                usersQuery =
                    (from u in usersQuery
                     join ur in dbContext.UserRoles.AsNoTracking() on u.Id equals ur.UserId
                     join r in dbContext.Roles.AsNoTracking() on ur.RoleId equals r.Id
                     where r.Name != null && r.Name.ToLower().Contains(roleFilter)
                     select u)
                    .Distinct();
            }

            var paginatedUsers = await PaginatedResponse<Domain.Entities.Identity.User>
                .CreateAsync(
                    usersQuery.OrderBy(u => u.UserName),
                    request.PageIndex,
                    request.PageSize,
                    ct
                );

            var users = paginatedUsers.Items
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    PhoneNumber = u.PhoneNumber,
                    IsActive = u.IsActive ?? true,
                    RoleNames = new List<string>()
                })
                .ToList();

            if (users.Count == 0)
            {
                return new GetAllUsersResponse
                {
                    Users = new PaginatedResponse<UserDto>(
                        paginatedUsers.PageIndex,
                        paginatedUsers.PageSize,
                        paginatedUsers.TotalCount,
                        users
                    )
                };
            }

            var userIds = users.Select(u => u.Id).ToList();

            var userRoleRows = await (
                from ur in dbContext.UserRoles.AsNoTracking()
                join r in dbContext.Roles.AsNoTracking() on ur.RoleId equals r.Id
                where userIds.Contains(ur.UserId)
                select new
                {
                    ur.UserId,
                    RoleName = r.Name
                }
            ).ToListAsync(ct);

            var rolesByUserId = userRoleRows
                .Where(x => !string.IsNullOrWhiteSpace(x.RoleName))
                .GroupBy(x => x.UserId)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.RoleName!)
                        .Distinct()
                        .OrderBy(name => name)
                        .ToList()
                );

            foreach (var user in users)
            {
                user.RoleNames = rolesByUserId.TryGetValue(user.Id, out var roleNames)
                    ? roleNames
                    : new List<string>();
            }

            return new GetAllUsersResponse
            {
                Users = new PaginatedResponse<UserDto>(
                    paginatedUsers.PageIndex,
                    paginatedUsers.PageSize,
                    paginatedUsers.TotalCount,
                    users
                )
            };
        }
    }
}
