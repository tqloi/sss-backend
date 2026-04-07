using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyAnswers.DeleteAnswer;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.DeleteAnswer
{
    public class DeleteAnswerValidator : Validator<DeleteAnswerCommand>
    {
        public DeleteAnswerValidator()
        {
            RuleFor(x => x.AnswerId)
                .GreaterThan(0)
                .WithMessage("AnswerId must be greater than 0");
        }
    }
}