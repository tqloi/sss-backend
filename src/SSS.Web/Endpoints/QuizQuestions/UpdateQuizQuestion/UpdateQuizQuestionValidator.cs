using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.UpdateQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.UpdateQuizQuestion
{
    public class UpdateQuizQuestionValidator : Validator<UpdateQuizQuestionCommand>
    {
        public UpdateQuizQuestionValidator() 
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
            //RuleFor(x => x.Question).NotEmpty().WithMessage("Question is required");
        }
    }
}
