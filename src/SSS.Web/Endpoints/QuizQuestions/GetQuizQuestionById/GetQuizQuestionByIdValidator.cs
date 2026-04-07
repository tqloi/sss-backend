using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.GetQuizQuestionById;

namespace SSS.Web.Endpoints.QuizQuestions.GetQuizQuestionById
{
    public class GetQuizQuestionByIdValidator : Validator<GetQuizQuestionByIdQuery>
    {
        public GetQuizQuestionByIdValidator()
        {
                RuleFor(x => x.id).NotEmpty().WithMessage("Id is required");
        }
    }
}
