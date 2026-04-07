using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.CheckRoadmapLimit
{
    public class CheckRoadmapLimitEndpoint(IAppDbContext db)
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

            var joinedRoadmaps = await db.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.UserId == userId && sp.Status != StudyPlanStatus.Archived)
                .Select(sp => sp.RoadmapId)
                .Distinct()
                .CountAsync(ct);

            await SendOkAsync(new CheckRoadmapLimitResponse
            {
                MaxRoadmaps = MaxJoinedRoadmaps,
                JoinedRoadmaps = joinedRoadmaps,
                HasReachedLimit = joinedRoadmaps >= MaxJoinedRoadmaps
            }, ct);
        }
    }
}
