using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudySessions.StartSession;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudySessions.StartSession
{
    public class StartSessionRequest
    {
        public long? StudyPlanId { get; set; }
        public long? NodeId { get; set; }
        public long? ModuleId { get; set; }
        public long[]? TaskIds { get; set; }
        public int? PlannedDurationSeconds { get; set; }
        public string? Timezone { get; set; }
    }

    public class StartSessionEndpoint(ISender sender, IHttpContextAccessor httpContext)
        : Endpoint<StartSessionRequest, StartSessionResult>
    {
        public override void Configure()
        {
            Post("/api/study-sessions/start");
            Description(d => d.WithTags("StudySessions"));
            Summary(s => s.Summary = "Start a new study session");
        }

        public override async Task HandleAsync(StartSessionRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);

            var command = new StartSessionCommand
            {
                UserId = userId!,
                StudyPlanId = req.StudyPlanId,
                NodeId = req.NodeId,
                ModuleId = req.ModuleId,
                TaskIds = req.TaskIds,
                PlannedDurationSeconds = req.PlannedDurationSeconds,
                Timezone = req.Timezone
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
