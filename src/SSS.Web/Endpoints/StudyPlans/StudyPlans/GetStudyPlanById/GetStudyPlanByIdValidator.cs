using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.GetStudyPlanById
{
    public class GetStudyPlanByIdValidator : Validator<GetStudyPlanByIdRequest>
    {
        public GetStudyPlanByIdValidator()
        {
            RuleFor(x => x.StudyPlanId)
                .GreaterThan(0)
                .WithMessage("Invalid Study Plan ID");
        }
    }
}
