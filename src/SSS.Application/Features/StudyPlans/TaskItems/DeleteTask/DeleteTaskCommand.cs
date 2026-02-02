using MediatR;

namespace SSS.Application.Features.StudyPlans.TaskItems.DeleteTask
{
    public class DeleteTaskCommand : IRequest<DeleteTaskResult>
    {
        public long TaskId { get; set; }
    }
}
