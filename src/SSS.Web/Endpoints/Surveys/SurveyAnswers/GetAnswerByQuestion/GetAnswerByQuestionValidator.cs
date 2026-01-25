using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyAnswers.GetAnswerByQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.GetAnswerByQuestion
{
    public class GetAnswerByQuestionValidator : Validator<GetAnswerByQuestionQuery>
    {
        public GetAnswerByQuestionValidator()
        {
            RuleFor(x => x.ResponseId)
                .GreaterThan(0)
                .WithMessage("ResponseId must be greater than 0");

            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .WithMessage("QuestionId must be greater than 0");
        }
    }
}