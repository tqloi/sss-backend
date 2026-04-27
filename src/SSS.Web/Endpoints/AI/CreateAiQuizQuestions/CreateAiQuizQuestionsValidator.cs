using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.AI.CreateAiQuizQuestions;

namespace SSS.Web.Endpoints.AI.CreateAiQuizQuestions
{
    public class CreateAiQuizQuestionsValidator : Validator<CreateAiQuizQuestionsCommand>
    {
        public CreateAiQuizQuestionsValidator()
        {
            RuleFor(x => x.QuizId)
                .GreaterThan(0).WithMessage("QuizId must be greater than 0.");

            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");

            RuleFor(x => x.RoadmapNodeId)
                .GreaterThan(0).WithMessage("RoadmapNodeId must be greater than 0.");

            RuleFor(x => x.Level)
                .NotEmpty().WithMessage("Level is required.")
                .MaximumLength(50).WithMessage("Level cannot exceed 50 characters.");

            RuleFor(x => x.QuestionCount)
                .InclusiveBetween(1, 10).WithMessage("QuestionCount must be between 1 and 20.");
        }
    }
}
