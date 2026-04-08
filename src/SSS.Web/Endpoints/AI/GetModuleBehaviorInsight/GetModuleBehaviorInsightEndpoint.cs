using FastEndpoints;
using MediatR;
using SSS.Application.Features.AI.GetModuleBehaviorInsight;
using System.Security.Claims;

namespace SSS.Web.Endpoints.AI.GetModuleBehaviorInsight
{
    public sealed class GetModuleBehaviorInsightEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetModuleBehaviorInsightQuery, GetModuleBehaviorInsightResult>
    {
        public override void Configure()
        {
            Get("/api/ai/module-behavior-insight/{StudyPlanId}");
        }

        public override async Task HandleAsync(GetModuleBehaviorInsightQuery req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var query = req with { UserId = userId };

            var response = await sender.Send(query, ct);
            await SendAsync(response, cancellation: ct);
        }
    }
}
