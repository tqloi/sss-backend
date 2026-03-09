using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.EditSurveyTriggerType;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.EditSurveyTriggerType
{
    public class EditSurveyTriggerTypeValidator : Validator<EditSurveyTriggerTypeCommand>
    {
        public EditSurveyTriggerTypeValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.");

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("DisplayName is required.")
                .MaximumLength(200).WithMessage("DisplayName must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
