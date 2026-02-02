using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.StudyPlans.TaskItems.CreateTask;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.CreateTask
{
    public class CreateTaskValidator : Validator<CreateTaskRequest>
    {
        public CreateTaskValidator()
        {
            RuleFor(x => x.StudyPlanModuleId)
                .GreaterThan(0)
                .WithMessage("Invalid Study Plan Module ID");

            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Title is required")
                .MaximumLength(500)
                .WithMessage("Title must not exceed 500 characters");

            RuleFor(x => x.EstimatedDurationSeconds)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Estimated duration must be non-negative");
        }
    }
}
