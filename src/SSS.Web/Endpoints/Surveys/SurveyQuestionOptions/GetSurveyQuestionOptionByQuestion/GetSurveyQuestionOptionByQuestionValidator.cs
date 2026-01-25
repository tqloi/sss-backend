using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionByQuestion
{
    public class GetSurveyQuestionOptionByQuestionValidator: Validator<GetSurveyQuestionOptionByQuestionQuery>
    {
        public GetSurveyQuestionOptionByQuestionValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .WithMessage("SurveyId must be greater than 0.");
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("PageIndex must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
