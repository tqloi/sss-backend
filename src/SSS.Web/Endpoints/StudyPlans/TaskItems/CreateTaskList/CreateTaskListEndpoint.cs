using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.CreateTaskList;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTaskList
{
    public class CreateTaskListEndpoint(ISender sender) 
        : Endpoint<CreateTaskListRequest, CreateTaskListResult>
    {
        public override void Configure()
        {
            Post("/api/tasks/batch");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Create multiple tasks at once");
        }

        public override async Task HandleAsync(CreateTaskListRequest req, CancellationToken ct)
        {
            var command = new CreateTaskListCommand
            {
                Tasks = req.Tasks
            };

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
