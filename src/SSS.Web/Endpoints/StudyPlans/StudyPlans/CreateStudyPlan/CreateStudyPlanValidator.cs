using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.StudyPlans.CreateStudyPlan
{
    public class CreateStudyPlanValidator : Validator<CreateStudyPlanRequest>
    {
        public CreateStudyPlanValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0)
                .WithMessage("Invalid Roadmap ID");
        }
    }
}
