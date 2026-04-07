using FastEndpoints;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.CheckRoadmapLimit
{
    public class CheckRoadmapLimitEndpoint(IAppDbContext db, IStudyPlanService studyPlanService)
        : EndpointWithoutRequest<CheckRoadmapLimitResponse>
    {
        private const int MaxJoinedRoadmaps = 2;

        public override void Configure()
        {
            Get("/api/study-plans/check-roadmap-limit");
            Description(d => d.WithTags("StudyPlans"));
            Summary(s => s.Summary = "Check whether current user has reached roadmap join limit");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var (joinedRoadmaps, hasReachedLimit) = await studyPlanService.CheckRoadmapLimitAsync(userId, MaxJoinedRoadmaps, ct);

            await SendOkAsync(new CheckRoadmapLimitResponse
            {
                MaxRoadmaps = MaxJoinedRoadmaps,
                JoinedRoadmaps = joinedRoadmaps,
                HasReachedLimit = hasReachedLimit
            }, ct);
        }
    }
}
