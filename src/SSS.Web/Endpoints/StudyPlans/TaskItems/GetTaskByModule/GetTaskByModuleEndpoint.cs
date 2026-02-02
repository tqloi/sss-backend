using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.GetTaskByModule;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.GetTaskByModule
{
    public class GetTaskByModuleEndpoint(ISender sender) 
        : Endpoint<GetTaskByModuleRequest, GetTaskByModuleResult>
    {
        public override void Configure()
        {
            Get("/api/tasks/by-module/{studyPlanModuleId}");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Get all tasks by study plan module");
        }

        public override async Task HandleAsync(GetTaskByModuleRequest req, CancellationToken ct)
        {
            var studyPlanModuleId = Route<long>("studyPlanModuleId");

            var query = new GetTaskByModuleQuery
            {
                StudyPlanModuleId = studyPlanModuleId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
