using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserManagement.GetAllRoles;

namespace SSS.Web.Endpoints.UserManagement.GetAllRoles
{
    public sealed class GetAllRolesEndpoint(ISender sender)
        : EndpointWithoutRequest<GetAllRolesResponse>
    {
        public override void Configure()
        {
            Get("/api/admin/users/roles");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Get all user roles";
                s.Description = "Returns all available roles for user filtering.";
            });
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await sender.Send(new GetAllRolesQuery(), ct);
            await SendOkAsync(response, ct);
        }
    }
}
