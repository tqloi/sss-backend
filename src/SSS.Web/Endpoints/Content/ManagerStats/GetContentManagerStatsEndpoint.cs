using FastEndpoints;
using MediatR;
using SSS.Application.Features.Content.ManagerStats;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Content.ManagerStats
{
    public sealed class GetContentManagerStatsEndpoint(ISender sender)
        : Endpoint<GetContentManagerStatsQuery, GetContentManagerStatsResult>
    {
        public override void Configure()
        {
            Get("/api/content-manager/stats");
            Summary(s => s.Summary = "Get dashboard statistics for the logged-in content manager");
            Description(d => d.WithTags("Content"));
            Roles("ContentManager");
        }

        public override async Task HandleAsync(GetContentManagerStatsQuery req, CancellationToken ct)
        {
            var managerId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(managerId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            req.ManagerId = managerId;

            var result = await sender.Send(req, ct);
            await SendOkAsync(result, ct);
        }
    }
}
