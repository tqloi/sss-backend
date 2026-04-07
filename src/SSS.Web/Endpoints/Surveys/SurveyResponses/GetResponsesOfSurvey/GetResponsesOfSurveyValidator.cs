using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyResponses.GetResponsesOfSurvey;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.GetResponsesOfSurvey
{
    public class GetResponsesOfSurveyValidator : Validator<GetResponseOfSurveyQuery>
    {
      public GetResponsesOfSurveyValidator()
        {
            RuleFor(x => x.surveyId)
                .GreaterThan(0).WithMessage("SurveyId must be greater than 0.");
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(0).WithMessage("PageIndex must be greater than or equal to 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.");
        }
    }
}
