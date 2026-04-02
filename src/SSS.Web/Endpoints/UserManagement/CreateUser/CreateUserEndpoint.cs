using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserManagement.CreateUser;
using System.Security.Claims;

namespace SSS.Web.Endpoints.UserManagement.CreateUser
{
    public sealed class CreateUserEndpoint(ISender sender)
        : Endpoint<CreateUserRequest, CreateUserResponse>
    {
        public override void Configure()
        {
            Post("/api/admin/users");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Create a user with selected role";
                s.Description = "Creates a new user account and assigns the selected role.";
            });
        }

        public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
        {
            var result = await sender.Send(new CreateUserCommand
            {
                Email = req.Email,
                Password = req.Password,
                ConfirmPassword = req.ConfirmPassword,
                FirstName = req.FirstName,
                LastName = req.LastName,
                RoleName = req.RoleName,
                CreatedBy = User.FindFirstValue(ClaimTypes.NameIdentifier),
            }, ct);

            await SendOkAsync(new CreateUserResponse
            {
                Message = "User account created successfully.",
                UserId = result.UserId,
                Email = result.Email,
                RoleName = result.RoleName,
            }, ct);
        }
    }
}
