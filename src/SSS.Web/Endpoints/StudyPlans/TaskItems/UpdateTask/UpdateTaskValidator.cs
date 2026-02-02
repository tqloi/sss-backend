using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.StudyPlans.TaskItems.UpdateTask
{
    public class UpdateTaskValidator : Validator<UpdateTaskRequest>
    {
        public UpdateTaskValidator()
        {
            RuleFor(x => x.TaskId)
                .GreaterThan(0)
                .WithMessage("Invalid Task ID");

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title)
                    .NotEmpty()
                    .WithMessage("Title cannot be empty")
                    .MaximumLength(500)
                    .WithMessage("Title must not exceed 500 characters");
            });

            When(x => x.EstimatedDurationSeconds.HasValue, () =>
            {
                RuleFor(x => x.EstimatedDurationSeconds!.Value)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("Estimated duration must be non-negative");
            });
        }
    }
}
