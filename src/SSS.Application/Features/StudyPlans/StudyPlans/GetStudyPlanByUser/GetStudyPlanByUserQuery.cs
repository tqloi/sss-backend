using MediatR;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanByUser
{
    public class GetStudyPlanByUserQuery : IRequest<GetStudyPlanByUserResult>
    {
        public string UserId { get; set; } = null!;
    }
}
