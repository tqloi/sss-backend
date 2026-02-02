using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.StudyPlans.CreateStudyPlan;
using System.Security.Claims;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.CreateStudyPlan
{
    public class CreateStudyPlanEndpoint(ISender sender, IHttpContextAccessor httpContext) 
        : Endpoint<CreateStudyPlanRequest, CreateStudyPlanResult>
    {
        public override void Configure()
        {
            Post("/api/study-plans");
            Description(d => d.WithTags("StudyPlans"));
            Summary(s => s.Summary = "Create a new study plan from roadmap");
        }

        public override async Task HandleAsync(CreateStudyPlanRequest req, CancellationToken ct)
        {
            var userId = httpContext.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            var command = new CreateStudyPlanCommand
            {
                UserId = userId!,
                RoadmapId = req.RoadmapId
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
