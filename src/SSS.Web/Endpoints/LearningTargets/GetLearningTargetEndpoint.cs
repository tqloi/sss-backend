using FastEndpoints;
using MediatR;
using SSS.Application.Features.LearningTargets.GetLearningTarget;
using System.Security.Claims;

namespace SSS.Web.Endpoints.LearningTargets
{
    public class GetLearningTargetEndpoint(IMediator mediator) : Endpoint<GetLearningTargetRequest, GetLearningTargetResult>
    {
        public override void Configure()
        {
            Get("/api/learning-targets/{RoadmapId}");
            Summary(s => {
                s.Summary = "Get User Learning Target";
                s.Description = "Retrieves the active user learning target (role, level, deadline) for a specific roadmap.";
            });
        }

        public override async Task HandleAsync(GetLearningTargetRequest req, CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var query = new GetLearningTargetQuery
            {
                UserId = userId,
                RoadmapId = req.RoadmapId
            };

            var result = await mediator.Send(query, ct);

            if (result == null)
            {
                await SendNotFoundAsync(ct);
                return;
            }

            await SendOkAsync(result, ct);
        }
    }

    public class GetLearningTargetRequest
    {
        public long RoadmapId { get; set; }
    }
}
