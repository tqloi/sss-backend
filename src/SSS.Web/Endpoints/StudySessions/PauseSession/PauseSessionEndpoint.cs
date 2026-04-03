using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.PauseSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.PauseSession
{
    public class PauseSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : EndpointWithoutRequest<PauseSessionResult>
    {
        public override void Configure()
        {
            Patch("/api/study-sessions/{Id}/pause");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Pause a study session");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var sessionId = Route<string>("Id");
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                AddError(r => r, "Id is required.");
                await SendErrorsAsync(cancellation: ct);
                return;
            }

            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new PauseSessionCommand
            {
                UserId = userId!,
                SessionId = sessionId
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
