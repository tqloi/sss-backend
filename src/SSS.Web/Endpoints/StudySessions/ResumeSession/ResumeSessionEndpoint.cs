using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.ResumeSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.ResumeSession
{
    public class ResumeSessionRequest
    {
        public string Id { get; set; } = null!;
    }

    public class ResumeSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<ResumeSessionRequest, ResumeSessionResult>
    {
        public override void Configure()
        {
            Patch("/api/study-sessions/{Id}/resume");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Resume a paused study session");
        }

        public override async Task HandleAsync(ResumeSessionRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new ResumeSessionCommand
            {
                UserId = userId!,
                SessionId = req.Id
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
