using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserManagement.ActivateUser;

namespace SSS.Web.Endpoints.UserManagement.ActivateUser
{
    public sealed class ActivateUserEndpoint(ISender sender)
        : Endpoint<ActivateUserCommand>
    {
        public override void Configure()
        {
            Put("/api/admin/users/{id}/activate");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Activate a user";
                s.Description = "Activates a user account and allows login again.";
            });
        }

        public override async Task HandleAsync(ActivateUserCommand req, CancellationToken ct)
        {
            req.Id = Route<string>("id") ?? req.Id;

            if (string.IsNullOrWhiteSpace(req.Id))
            {
                await SendBadRequestAsync(ct);
                return;
            }

            var activated = await sender.Send(req, ct);

            if (!activated)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendNoContentAsync(ct);
        }
    }
}
