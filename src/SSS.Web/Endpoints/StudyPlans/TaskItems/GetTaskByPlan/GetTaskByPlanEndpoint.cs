using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.GetTaskByPlan;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.GetTaskByPlan
{
    public class GetTaskByPlanEndpoint(ISender sender) 
        : Endpoint<GetTaskByPlanRequest, GetTaskByPlanResult>
    {
        public override void Configure()
        {
            Get("/api/tasks/by-plan/{studyPlanId}");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Get all tasks by study plan");
        }

        public override async Task HandleAsync(GetTaskByPlanRequest req, CancellationToken ct)
        {
            var studyPlanId = Route<long>("studyPlanId");

            var query = new GetTaskByPlanQuery
            {
                StudyPlanId = studyPlanId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
