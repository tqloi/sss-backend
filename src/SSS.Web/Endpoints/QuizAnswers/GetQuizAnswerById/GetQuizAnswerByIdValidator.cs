using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAnswers.GetQuizAnswerById;

namespace SSS.Web.Endpoints.QuizAnswers.GetQuizAnswerById
{
    public class GetQuizAnswerByIdValidator : Validator<GetQuizAnswerByIdQuery>
    {
        public GetQuizAnswerByIdValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Quiz answer ID is required.")
                .GreaterThan(0).WithMessage("Quiz answer ID must be greater than 0");
        }
    }
}
