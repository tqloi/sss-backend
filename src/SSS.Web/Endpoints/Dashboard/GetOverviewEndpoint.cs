using FastEndpoints;
using MediatR;
using SSS.Application.Features.Dashboard.GetOverview;
using System.Security.Claims;

namespace SSS.Web.Endpoints.Dashboard
{
    public class GetOverviewEndpoint(IMediator mediator) : Endpoint<GetOverviewRequest, GetOverviewResult>
    {
        public override void Configure()
        {
            Get("/api/dashboard/{StudyPlanId}");
            Summary(s => {
                s.Summary = "Get Dashboard Overview Data";
                s.Description = "Retrieves stats, current focus, upcoming tasks and recent sessions for the dashboard view.";
            });
        }

        public override async Task HandleAsync(GetOverviewRequest req, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var query = new GetOverviewQuery
            {
                UserId = userId,
                StudyPlanId = req.StudyPlanId
            };

            var result = await mediator.Send(query, ct);

            await SendOkAsync(result, ct);
        }
    }

    public class GetOverviewRequest
    {
        public long StudyPlanId { get; set; }
    }
}
