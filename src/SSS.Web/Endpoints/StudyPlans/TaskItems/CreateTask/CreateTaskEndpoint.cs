using FastEndpoints;
using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.CreateTask;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTask
{
    public class CreateTaskEndpoint(ISender sender, AutoMapper.IMapper mapper) 
        : Endpoint<CreateTaskRequest, CreateTaskResult>
    {
        public override void Configure()
        {
            Post("/api/tasks");
            Description(d => d.WithTags("TaskItems"));
            Summary(s => s.Summary = "Create a new task");
        }

        public override async Task HandleAsync(CreateTaskRequest req, CancellationToken ct)
        {
            var command = mapper.Map<CreateTaskCommand>(req);

            var result = await sender.Send(command, ct);
            await SendOkAsync(result, ct);
        }
    }
}
