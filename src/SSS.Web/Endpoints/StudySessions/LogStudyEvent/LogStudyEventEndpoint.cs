using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.LogStudyEvent;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.LogStudyEvent
{
    public class LogStudyEventRequest
    {
        public string Id { get; set; } = null!;
        public string EventType { get; set; } = null!;
        public long? TaskId { get; set; }
        public string? UserId { get; set; }
        public string? StudyPlanModuleId { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
    }

    public class LogStudyEventEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<LogStudyEventRequest, LogStudyEventResult>
    {
        public override void Configure()
        {
            Post("/api/study-sessions/{Id}/events");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Log a study event within a session");
        }

        public override async Task HandleAsync(LogStudyEventRequest req, CancellationToken ct)
        {
            var userId = req.UserId ?? httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new LogStudyEventCommand
            {
                UserId = userId!,
                SessionId = req.Id,
                EventType = req.EventType,
                TaskId = req.TaskId,
                StudyPlanModuleId = req.StudyPlanModuleId,
                Metadata = req.Metadata
            };

            var result = await sender.Send(command, ct);
            await SendAsync(result, 201, ct);
        }
    }
}
