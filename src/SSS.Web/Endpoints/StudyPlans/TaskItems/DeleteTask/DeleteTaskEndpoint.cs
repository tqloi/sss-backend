using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.DeleteTask;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.DeleteTask
{
    public class DeleteTaskEndpoint(ISender sender) 
        : Endpoint<DeleteTaskRequest, DeleteTaskResult>
    {
        public override void Configure()
        {
            Delete("/api/tasks/{taskId}");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Delete a task");
        }

        public override async Task HandleAsync(DeleteTaskRequest req, CancellationToken ct)
        {
            var taskId = Route<long>("taskId");

            var command = new DeleteTaskCommand
            {
                TaskId = taskId
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
