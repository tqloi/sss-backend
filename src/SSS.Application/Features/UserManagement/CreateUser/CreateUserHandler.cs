using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Entities.Identity;
using System.ComponentModel.DataAnnotations;

namespace SSS.Application.Features.UserManagement.CreateUser
{
    public sealed class CreateUserHandler(
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager)
        : IRequestHandler<CreateUserCommand, CreateUserResult>
    {
        public async Task<CreateUserResult> Handle(CreateUserCommand request, CancellationToken ct)
        {
            if (!string.Equals(request.Password, request.ConfirmPassword, StringComparison.Ordinal))
            {
                throw new ValidationException("Passwords do not match.");
            }

            var normalizedRoleName = request.RoleName?.Trim();
            if (string.IsNullOrWhiteSpace(normalizedRoleName))
            {
                throw new ValidationException("Role is required.");
            }

            var role = await roleManager.Roles
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Name != null && x.Name.ToLower() == normalizedRoleName.ToLower(),
                    ct);

            if (role?.Name is null)
            {
                throw new ValidationException("Selected role does not exist.");
            }

            var existed = await userManager.FindByEmailAsync(request.Email);
            if (existed is not null)
            {
                throw new ConflictException("Email already exists");
            }

            var user = new User
            {
                UserName = request.Email,
                AvatarUrl = "",
                FirstName = request.FirstName,
                LastName = request.LastName ?? "",
                Email = request.Email,
                EmailConfirmed = true,
                IsActive = true,
            };

            var create = await userManager.CreateAsync(user, request.Password);
            if (!create.Succeeded)
            {
                throw new ValidationException(create.Errors.First().Description);
            }

            var addRole = await userManager.AddToRoleAsync(user, role.Name);
            if (!addRole.Succeeded)
            {
                await userManager.DeleteAsync(user);
                throw new ValidationException(addRole.Errors.First().Description);
            }

            return new CreateUserResult
            {
                UserId = user.Id,
                Email = user.Email!,
                RoleName = role.Name,
            };
        }
    }
}
