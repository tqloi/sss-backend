using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.DeleteTask
{
    public class DeleteTaskValidator : Validator<DeleteTaskRequest>
    {
        public DeleteTaskValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0)
                .WithMessage("Invalid Task ID");
        }
    }
}
