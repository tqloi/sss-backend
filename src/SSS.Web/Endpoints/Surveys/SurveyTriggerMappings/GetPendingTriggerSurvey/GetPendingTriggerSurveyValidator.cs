using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey
{
    public class GetPendingTriggerSurveyValidator : Validator<GetPendingTriggerSurveyRequest>
    {
        public GetPendingTriggerSurveyValidator()
        {
            RuleFor(x => x.TriggerType)
                .NotEmpty().WithMessage("TriggerType is required.");
        }
    }
}
