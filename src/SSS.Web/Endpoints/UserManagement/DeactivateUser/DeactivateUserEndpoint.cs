using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserManagement.DeactivateUser;

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

            if (string.IsNullOrWhiteSpace(req.Id))
            {
                await SendBadRequestAsync(ct);
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
