using MediatR;
using SSS.Application.Features.StudyPlans.TaskItems.Common;

namespace SSS.Application.Features.StudyPlans.TaskItems.CreateTaskList
{
    public class CreateTaskListCommand : IRequest<CreateTaskListResult>
    {
        public List<TastItemInput> Tasks { get; set; } = new();
    }
}
