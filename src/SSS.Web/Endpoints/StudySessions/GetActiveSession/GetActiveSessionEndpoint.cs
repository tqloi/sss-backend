using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.GetActiveSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.GetActiveSession
{
    public class GetActiveSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : EndpointWithoutRequest<GetActiveSessionResult>
    {
        public override void Configure()
        {
            Get("/api/study-sessions/active");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Get the current user's active session (if any)");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var query = new GetActiveSessionQuery { UserId = userId! };
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
