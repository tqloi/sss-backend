using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.ResumeSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.ResumeSession
{
    public class ResumeSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : EndpointWithoutRequest<ResumeSessionResult>
    {
        public override void Configure()
        {
            Patch("/api/study-sessions/{Id}/resume");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Resume a paused study session");
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

            var command = new ResumeSessionCommand
            {
                UserId = userId!,
                SessionId = sessionId
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
