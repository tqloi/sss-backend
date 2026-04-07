using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.GetAllQuizQuestionsByQuizId;

namespace SSS.Web.Endpoints.QuizQuestions.GetAllQuizQuestionsByQuizId
{
    public class GetAllQuizQuestionsByQuizIdValidator : Validator<GetAllQuizQuestionsByQuizIdQuery>
    {
        public GetAllQuizQuestionsByQuizIdValidator()
        {
            RuleFor(x => x.quizId)
                .GreaterThan(0).WithMessage("quizId must be greater than 0.");
        }
    }
}
