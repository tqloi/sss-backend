using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType
{
    public class CreateSurveyTriggerTypeValidator : Validator<CreateSurveyTriggerTypeCommand>
    {
        public CreateSurveyTriggerTypeValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Code is required.")
                .MaximumLength(100).WithMessage("Code must not exceed 100 characters.");

            RuleFor(x => x.DisplayName)
                .NotEmpty().WithMessage("DisplayName is required.")
                .MaximumLength(200).WithMessage("DisplayName must not exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");
        }
    }
}
