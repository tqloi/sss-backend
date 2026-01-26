using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.CreateQuiz;

namespace SSS.Web.Endpoints.Quizzes.CreateQuiz
{
    public sealed class CreateQuizValidator : Validator<CreateQuizCommand>
    {
        public CreateQuizValidator() 
        {
            RuleFor(x => x.CreateQuizNode.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");
            RuleFor(x => x.CreateQuizNode.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");
            RuleFor(x => x.CreateQuizNode.StudyPlanModuleId)
                .NotNull().WithMessage("StudyPlanModuleId is required.")
                .GreaterThan(0).WithMessage("StudyPlanModuleId must be greater than 0.");

        }
    }
}
