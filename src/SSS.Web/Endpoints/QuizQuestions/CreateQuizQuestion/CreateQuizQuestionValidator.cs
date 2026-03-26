using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.CreateQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.CreateQuizQuestion
{
    public class CreateQuizQuestionValidator : Validator<CreateQuizQuestionCommand>
    {
        public CreateQuizQuestionValidator() 
        {
            RuleFor(q => q.CreateQuizQuestionDto.QuizId)
                .GreaterThan(0).WithMessage("QuizId must be greater than 0.");

            RuleFor(q => q.CreateQuizQuestionDto.Level)
                .NotEmpty().WithMessage("Level is required.")
                .MaximumLength(50).WithMessage("Level cannot exceed 50 characters.");

            RuleFor(q => q.CreateQuizQuestionDto.QuestionKey)
                .NotEmpty().WithMessage("QuestionKey is required.")
                .MaximumLength(100).WithMessage("QuestionKey cannot exceed 100 characters.");

            RuleFor(q => q.CreateQuizQuestionDto.Prompt)
                .NotEmpty().WithMessage("Prompt is required.");
        }
    }
}
