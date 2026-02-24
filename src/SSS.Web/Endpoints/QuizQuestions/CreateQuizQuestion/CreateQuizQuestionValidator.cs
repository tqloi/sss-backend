using FastEndpoints;
using SSS.Application.Features.QuizQuestions.CreateQuizQuestion;

namespace SSS.Web.Endpoints.QuizQuestions.CreateQuizQuestion
{
    public class CreateQuizQuestionValidator : Validator<CreateQuizQuestionCommand>
    {
        public CreateQuizQuestionValidator() 
        {
            //RuleFor(q => q.CreateQuizQuestionDto.)
            //    .NotEmpty().WithMessage("QuizId is required.");
            //RuleFor(q => q.Text)
            //    .NotEmpty().WithMessage("Question text is required.");
        }
    }
}
