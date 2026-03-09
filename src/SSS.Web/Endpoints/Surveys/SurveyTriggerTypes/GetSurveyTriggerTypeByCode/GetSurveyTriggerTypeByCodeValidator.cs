using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode
{
    public class GetSurveyTriggerTypeByCodeValidator : Validator<GetSurveyTriggerTypeByCodeQuery>
    {
        public GetSurveyTriggerTypeByCodeValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");
        }
    }
}
