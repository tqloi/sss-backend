using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.GetTaskById
{
    public class GetTaskByIdValidator : Validator<GetTaskByIdRequest>
    {
        public GetTaskByIdValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0)
                .WithMessage("Invalid Task ID");
        }
    }
}
