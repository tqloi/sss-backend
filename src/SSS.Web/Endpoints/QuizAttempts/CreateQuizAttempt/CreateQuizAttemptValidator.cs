using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAttempts.CreateQuizAttempt;

namespace SSS.Web.Endpoints.QuizAttempts.CreateQuizAttempt
{
    public class CreateQuizAttemptValidator : Validator<CreateQuizAttemptCommand>
    {
        private static readonly string[] SupportedLevels =
        [
            "Begineer",
            "Beginner",
            "Intermediate",
            "Advanced"
        ];

        public CreateQuizAttemptValidator()
        {
            RuleFor(x => x.CreateQuizAttempt.StudyPlanModuleId)
                .NotEmpty()
                .WithMessage("Study plan module ID is required.")
                .GreaterThan(0)
                .WithMessage("Study plan module ID must be a positive integer.");

            RuleFor(x => x.CreateQuizAttempt.Level)
                .NotEmpty()
                .WithMessage("Level is required.")
                .Must(level => SupportedLevels.Contains(level, StringComparer.OrdinalIgnoreCase))
                .WithMessage("Level must be one of: Begineer, Beginner, Intermediate, Advanced.");
        }
    }
}
