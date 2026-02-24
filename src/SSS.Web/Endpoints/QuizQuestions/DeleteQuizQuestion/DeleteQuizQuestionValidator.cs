using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.DeleteQuizQuestion;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.QuizQuestions.DeleteQuizQuestion
{
    public class DeleteQuizQuestionValidator : Validator<DeleteQuizQuestionCommand>
    {
        public DeleteQuizQuestionValidator() 
        {
            RuleFor(x => x.id)
                .NotEmpty().WithMessage("Quiz question id is required.")
                .GreaterThan(0).WithMessage("Quiz question id must be greater than 0.");
        }
    }
}
