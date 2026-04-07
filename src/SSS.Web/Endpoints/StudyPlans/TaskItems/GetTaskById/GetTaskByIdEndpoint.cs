using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.GetTaskById;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.GetTaskById
{
    public class GetTaskByIdEndpoint(ISender sender) 
        : Endpoint<GetTaskByIdRequest, GetTaskByIdResult>
    {
        public override void Configure()
        {
            Get("/api/tasks/{taskId}");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Get a task by ID");
        }

        public override async Task HandleAsync(GetTaskByIdRequest req, CancellationToken ct)
        {
            var taskId = Route<long>("taskId");

            var query = new GetTaskByIdQuery
            {
                TaskId = taskId
            };

            var result = await sender.Send(query, ct);
            await SendOkAsync(result, ct);
        }
    }
}
