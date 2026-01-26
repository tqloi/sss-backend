using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.DeleteQuiz;

namespace SSS.Web.Endpoints.Quizzes.DeleteQuiz
{
    public class DeleteQuizValidator : Validator<DeleteQuizRequest>
    {
        public DeleteQuizValidator() 
        {
            RuleFor(x => x.QuizId)
                .NotEmpty().WithMessage("QuizId is required.")
                .GreaterThan(0).WithMessage("QuizId must be greater than 0.");
        }
    }
}
