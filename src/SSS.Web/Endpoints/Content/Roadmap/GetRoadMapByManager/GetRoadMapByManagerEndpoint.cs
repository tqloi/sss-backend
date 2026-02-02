using FastEndpoints;
using MediatR;
using Microsoft.AspNetCore.Http;
using SSS.Application.Features.Content.Roadmap.GetRoadMapByManager;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Content.Roadmap.GetRoadMapByManager
{
    public class GetRoadMapByManagerEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetRoadMapByManagerQuery, GetRoadMapByManagerResult>
    {
        public override void Configure()
        {
            Get("/api/roadmaps/manager");
            Summary(s => s.Summary = "Get roadmaps created by logged-in manager with advanced filtering");
            Description(d => d.WithTags("Roadmaps"));
            Roles("Admin", "Manager");
        }

        public override async Task HandleAsync(
            GetRoadMapByManagerQuery req,
            CancellationToken ct)
        {
            var managerId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var query = req with
            {
                ManagerId = managerId!
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
