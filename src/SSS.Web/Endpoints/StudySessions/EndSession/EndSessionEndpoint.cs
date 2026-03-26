using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.EndSession;
using SSS.Application.Features.StudySessions.Common;
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
        public List<EndSessionTaskRequest>? Tasks { get; set; }
    }

    public class EndSessionTaskRequest
    {
        public long TaskId { get; set; }
        public DateTime? EndTime { get; set; }
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
                Tasks = req.Tasks?.Select(t => new EndSessionTaskDto
                {
                    TaskId = t.TaskId,
                    EndTime = t.EndTime
                }).ToList()
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
