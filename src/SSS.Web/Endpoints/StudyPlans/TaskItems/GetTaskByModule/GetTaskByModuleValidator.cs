using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.GetTaskByModule
{
    public class GetTaskByModuleValidator : Validator<GetTaskByModuleRequest>
    {
        public GetTaskByModuleValidator()
        {
            RuleFor(x => x.StudyPlanModuleId)
                .GreaterThan(0)
                .WithMessage("Invalid Study Plan Module ID");
        }
    }
}
