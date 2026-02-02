using MediatR;

namespace SSS.Application.Features.StudyPlans.TaskItems.GetTaskByModule
{
    public class GetTaskByModuleQuery : IRequest<GetTaskByModuleResult>
    {
        public long StudyPlanModuleId { get; set; }
    }
}
