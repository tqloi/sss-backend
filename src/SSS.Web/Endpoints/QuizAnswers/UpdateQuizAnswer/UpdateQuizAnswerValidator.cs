using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAnswers.UpdateQuizAnswer;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.QuizAnswers.UpdateQuizAnswer
{
    public class UpdateQuizAnswerValidator : Validator<UpdateQuizAnswerCommand>
    {
        public UpdateQuizAnswerValidator() 
        {
            RuleFor(q => q.Id)
                .GreaterThan(0).WithMessage("Quiz answer ID must be greater than 0.");
            RuleFor(q => q.UpdateQuizAnswer.TextValue)
                .NotEmpty().WithMessage("Text is required.")
                .MaximumLength(500).WithMessage("Answer text cannot exceed 500 characters.");
        }
    }
}
