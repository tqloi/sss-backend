using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.GetTaskByPlan
{
    public class GetTaskByPlanValidator : Validator<GetTaskByPlanRequest>
    {
        public GetTaskByPlanValidator()
        {
            RuleFor(x => x.StudyPlanId)
                .GreaterThan(0)
                .WithMessage("Invalid Study Plan ID");
        }
    }
}
