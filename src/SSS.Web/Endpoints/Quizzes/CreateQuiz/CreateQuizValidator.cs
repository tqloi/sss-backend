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

            RuleFor(x => x.CreateQuizNode.RoadmapNodeId)
                .GreaterThan(0).WithMessage("RoadmapNodeId must be greater than 0.");

            RuleFor(x => x.CreateQuizNode.Level)
                .NotEmpty().WithMessage("Level is required.")
                .MaximumLength(50).WithMessage("Level cannot exceed 50 characters.");

            RuleFor(x => x.CreateQuizNode.PassingScore)
                .GreaterThanOrEqualTo(0).WithMessage("PassingScore must be greater than or equal to 0.");

            RuleFor(x => x.CreateQuizNode)
                .Must(x => !x.TotalScore.HasValue || x.PassingScore <= x.TotalScore.Value)
                .WithMessage("PassingScore cannot be greater than TotalScore.");
        }
    }
}
