using MediatR;

namespace SSS.Application.Features.StudyPlans.TaskItems.GetTaskById
{
    public class GetTaskByIdQuery : IRequest<GetTaskByIdResult>
    {
        public long TaskId { get; set; }
    }
}
