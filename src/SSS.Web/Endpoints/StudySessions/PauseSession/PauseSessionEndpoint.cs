using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.PauseSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.PauseSession
{
    public class PauseSessionRequest
    {
        public string Id { get; set; } = null!;
    }

    public class PauseSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<PauseSessionRequest, PauseSessionResult>
    {
        public override void Configure()
        {
            Patch("/api/study-sessions/{Id}/pause");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Pause a study session");
        }

        public override async Task HandleAsync(PauseSessionRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new PauseSessionCommand
            {
                UserId = userId!,
                SessionId = req.Id
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
