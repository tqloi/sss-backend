using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTaskList
{
    public class CreateTaskListValidator : Validator<CreateTaskListRequest>
    {
        public CreateTaskListValidator()
        {
            RuleFor(x => x.Tasks)
                .NotEmpty()
                .WithMessage("Task list cannot be empty");

            RuleForEach(x => x.Tasks).ChildRules(task =>
            {
                task.RuleFor(t => t.StudyPlanModuleId)
                    .GreaterThan(0)
                    .WithMessage("Invalid Study Plan Module");

                task.RuleFor(t => t.Title)
                    .NotEmpty()
                    .WithMessage("Title is required")
                    .MaximumLength(500)
                    .WithMessage("Title must not exceed 500 characters");

                task.RuleFor(t => t.EstimatedDurationSeconds)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Estimated duration must be non-negative");
            });
        }
    }
}
