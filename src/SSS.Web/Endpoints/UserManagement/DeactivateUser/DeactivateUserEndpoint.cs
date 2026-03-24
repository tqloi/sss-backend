using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserManagement.DeactivateUser;
using System.Security.Claims;

namespace SSS.Web.Endpoints.UserManagement.DeactivateUser
{
    public sealed class DeactivateUserEndpoint(ISender sender)
        : Endpoint<DeactivateUserCommand>
    {
        public override void Configure()
        {
            Put("/api/admin/users/{id}/deactivate");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Deactivate a user";
                s.Description = "Deactivates a user account and blocks future login.";
            });
        }

        public override async Task HandleAsync(DeactivateUserCommand req, CancellationToken ct)
        {
            req.Id = Route<string>("id") ?? req.Id;
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrWhiteSpace(req.Id))
            {
                await SendAsync(new { message = "Invalid user id" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            if (!string.IsNullOrWhiteSpace(currentUserId) &&
                string.Equals(req.Id, currentUserId, StringComparison.Ordinal))
            {
                await SendAsync(new { message = "You cannot block your own account." }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            var deactivated = await sender.Send(req, ct);

            if (!deactivated)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendNoContentAsync(ct);
        }
    }
}
