using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.EditSurveyTriggerMapping;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.EditSurveyTriggerMapping
{
    public class EditSurveyTriggerMappingValidator : Validator<EditSurveyTriggerMappingCommand>
    {
        public EditSurveyTriggerMappingValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.SurveyId)
                .NotEmpty().WithMessage("Survey Id is required.")
                .GreaterThan(0).WithMessage("Survey Id must be greater than zero.");
            RuleFor(x => x.TriggerType)
                .NotEmpty().WithMessage("Trigger Event is required.");
        }
    }
}
