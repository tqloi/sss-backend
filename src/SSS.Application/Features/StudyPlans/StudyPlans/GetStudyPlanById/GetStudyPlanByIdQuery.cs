using MediatR;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanById
{
    public class GetStudyPlanByIdQuery : IRequest<GetStudyPlanByIdResult>
    {
        public long StudyPlanId { get; set; }
    }
}
