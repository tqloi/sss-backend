using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType
{
    public class DeleteSurveyTriggerTypeValidator : Validator<DeleteSurveyTriggerTypeCommand>
    {
        public DeleteSurveyTriggerTypeValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");
        }
    }
}
