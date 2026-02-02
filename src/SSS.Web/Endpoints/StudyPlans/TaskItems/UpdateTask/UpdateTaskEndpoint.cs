using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.UpdateTask;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.UpdateTask
{
    public class UpdateTaskEndpoint(ISender sender, AutoMapper.IMapper mapper) 
        : Endpoint<UpdateTaskRequest, UpdateTaskResult>
    {
        public override void Configure()
        {
            Put("/api/tasks/{taskId}");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Update a task");
        }

        public override async Task HandleAsync(UpdateTaskRequest req, CancellationToken ct)
        {
            var taskId = Route<long>("taskId");

            var command = mapper.Map<UpdateTaskCommand>(req);

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
