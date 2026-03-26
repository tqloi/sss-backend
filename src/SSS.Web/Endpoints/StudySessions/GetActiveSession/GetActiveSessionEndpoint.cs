using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetActiveSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetActiveSession
{
    public class GetActiveSessionRequest
    {
        [QueryParam] public long? PlanId { get; set; }
    }

    public class GetActiveSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<GetActiveSessionRequest, GetActiveSessionResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/active");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get the current user's active session (if any)");
        }

        public override async Task HandleAsync(GetActiveSessionRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetActiveSessionQuery { UserId = userId!, PlanId = req.PlanId };
            var result = await sender.Send(query, ct);

            if (result.Data == null)
            {
                await SendNoContentAsync(ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }
}
