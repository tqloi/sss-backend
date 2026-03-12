using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.EndSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.EndSession
{
    public class EndSessionRequest
    {
        public string Id { get; set; } = null!;
        public string? EndedReason { get; set; }
        public int? SelfRating { get; set; }
        public string? Notes { get; set; }
        public int? ActualDurationSeconds { get; set; }
        public int? ActiveSeconds { get; set; }
        public int? IdleSeconds { get; set; }
        public long[]? TasksCompleted { get; set; }
        public int? FocusScore { get; set; }
        public int? FatigueScore { get; set; }
    }

    public class EndSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<EndSessionRequest, EndSessionResult>
    {
        public override void Configure()
        {
            Patch("/api/study-sessions/{Id}/end");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "End a study session with summary data");
        }

        public override async Task HandleAsync(EndSessionRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new EndSessionCommand
            {
                UserId = userId!,
                SessionId = req.Id,
                EndedReason = req.EndedReason,
                SelfRating = req.SelfRating,
                Notes = req.Notes,
                ActualDurationSeconds = req.ActualDurationSeconds,
                ActiveSeconds = req.ActiveSeconds,
                IdleSeconds = req.IdleSeconds,
                TasksCompleted = req.TasksCompleted,
                FocusScore = req.FocusScore,
                FatigueScore = req.FatigueScore
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
