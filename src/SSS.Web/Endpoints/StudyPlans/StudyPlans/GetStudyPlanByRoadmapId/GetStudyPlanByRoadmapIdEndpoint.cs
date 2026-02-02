using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanByRoamapId;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.GetStudyPlanByRoadmapId
{
    public class GetStudyPlanByRoadmapIdEndpoint(ISender sender, IHttpContextAccessor httpContext) 
        : Endpoint<GetStudyPlanByRoadmapIdRequest, GetStudyPlanByRoadmapIdResult>
    {
        public override void Configure()
        {
            Get("/api/study-plans/by-roadmap/{RoadmapId}");
            Description(d => d.WithTags("StudyPlans"));
            Summary(s => s.Summary = "Get study plan by roadmap ID and user ID");
        }

        public override async Task HandleAsync(GetStudyPlanByRoadmapIdRequest req, CancellationToken ct)
        {
            var roadmapId = Route<long>("RoadmapId");
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var query = new GetStudyPlanByRoadmapIdQuery
            {
                UserId = userId!,
                RoadmapId = roadmapId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
