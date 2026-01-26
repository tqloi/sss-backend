using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.GetQuizById;

namespace SSS.Web.Endpoints.Quizzes.GetQuizById
{
    public class GetQuizByIdValidator : Validator<GetQuizByIdQuery>
    {
        public GetQuizByIdValidator()
        {
            RuleFor(x => x.id)
                .GreaterThan(0).WithMessage("QuizId must be greater than 0.");
        }
    }
}
