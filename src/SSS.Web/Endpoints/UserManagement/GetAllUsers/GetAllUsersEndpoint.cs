using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserManagement.GetAllUsers;

namespace SSS.Web.Endpoints.UserManagement.GetAllUsers
{
    public sealed class GetAllUsersEndpoint(ISender sender)
        : Endpoint<GetAllUsersQuery, GetAllUsersResponse>
    {
        public override void Configure()
        {
            Get("/api/admin/users");
            Roles("Admin");
            Description(d => d.WithTags("UserManagement"));
            Summary(s =>
            {
                s.Summary = "Get all users";
                s.Description = "Returns all users for admin management.";
            });
        }

        public override async Task HandleAsync(GetAllUsersQuery req, CancellationToken ct)
        {
            var response = await sender.Send(req, ct);
            await SendOkAsync(response, ct);
        }
    }
}
