using MediatR;

namespace SSS.Application.Features.StudyPlans.TaskItems.GetTaskByPlan
{
    public class GetTaskByPlanQuery : IRequest<GetTaskByPlanResult>
    {
        public long StudyPlanId { get; set; }
    }
}
