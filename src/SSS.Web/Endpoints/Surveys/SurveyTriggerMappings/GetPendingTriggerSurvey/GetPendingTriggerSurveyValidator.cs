using FastEndpoints;
using FluentValidation;
using SSS.Domain.Constants;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetPendingTriggerSurvey
{
    public class GetPendingTriggerSurveyValidator : Validator<GetPendingTriggerSurveyRequest>
    {
        private static readonly HashSet<string> _validTriggerTypes =
        [
            SurveyTriggerTypes.OnRegister,
            SurveyTriggerTypes.OnStartRoadmap,
            SurveyTriggerTypes.OnCompleteModule,
        ];

        public GetPendingTriggerSurveyValidator()
        {
            RuleFor(x => x.TriggerType)
                .NotEmpty().WithMessage("TriggerType is required.")
                .Must(t => _validTriggerTypes.Contains(t))
                .WithMessage(
                    $"TriggerType must be one of: " +
                    $"{SurveyTriggerTypes.OnRegister}, " +
                    $"{SurveyTriggerTypes.OnStartRoadmap}, " +
                    $"{SurveyTriggerTypes.OnCompleteModule}.");
        }
    }
}
