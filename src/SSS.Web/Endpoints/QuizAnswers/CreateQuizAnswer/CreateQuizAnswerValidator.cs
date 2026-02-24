using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAnswers.CreateQuizAnswer;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.QuizAnswers.CreateQuizAnswer
{
    public class CreateQuizAnswerValidator : Validator<CreateQuizAnswerCommand>
    {

        public CreateQuizAnswerValidator() 
        {
            RuleFor(x => x.CreateQuizAnswer.TextValue)
                .MaximumLength(1000).WithMessage("AnswerText cannot exceed 1000 characters.");
           
        }

    }
}
