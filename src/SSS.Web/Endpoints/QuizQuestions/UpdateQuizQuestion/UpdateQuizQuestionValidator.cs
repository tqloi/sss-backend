using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.UpdateQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.UpdateQuizQuestion
{
    public class UpdateQuizQuestionValidator : Validator<UpdateQuizQuestionCommand>
    {
        public UpdateQuizQuestionValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id is required");

            RuleFor(x => x.UpdateQuizQuestionDto.QuestionKey)
                .NotEmpty().WithMessage("QuestionKey is required")
                .MaximumLength(100).WithMessage("QuestionKey cannot exceed 100 characters.");

            RuleFor(x => x.UpdateQuizQuestionDto.Level)
                .NotEmpty().WithMessage("Level is required")
                .MaximumLength(50).WithMessage("Level cannot exceed 50 characters.");

            RuleFor(x => x.UpdateQuizQuestionDto.Prompt)
                .NotEmpty().WithMessage("Prompt is required");

            RuleFor(x => x.UpdateQuizQuestionDto.ScoreWeight)
                .GreaterThanOrEqualTo(0).WithMessage("ScoreWeight must be greater than or equal to 0.");

            RuleFor(x => x.UpdateQuizQuestionDto.OrderNo)
                .GreaterThanOrEqualTo(0).WithMessage("OrderNo must be greater than or equal to 0.");
        }
    }
}
