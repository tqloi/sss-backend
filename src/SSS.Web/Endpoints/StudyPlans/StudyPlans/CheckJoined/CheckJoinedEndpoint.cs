using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.CheckJoined
{
    public class CheckJoinedEndpoint(IAppDbContext db)
        : EndpointWithoutRequest<CheckJoinedResponse>
    {
        public override void Configure()
        {
            Get("/api/study-plans/check-joined/{studyPlanId:long}");
            Description(d => d.WithTags("StudyPlans"));
            Summary(s => s.Summary = "Check whether current user owns a study plan");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(userId))
            {
                await SendUnauthorizedAsync(ct);
                return;
            }

            var studyPlanId = Route<long>("studyPlanId");

            var ownerUserId = await db.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.Id == studyPlanId)
                .Select(sp => sp.UserId)
                .FirstOrDefaultAsync(ct);

            var isJoined = !string.IsNullOrWhiteSpace(ownerUserId) && ownerUserId == userId;
            await SendOkAsync(new CheckJoinedResponse { IsJoined = isJoined }, ct);
        }
    }
}
