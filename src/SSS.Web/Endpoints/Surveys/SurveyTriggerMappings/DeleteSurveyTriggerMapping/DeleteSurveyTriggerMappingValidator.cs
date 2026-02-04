using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.DeleteSurveyTriggerMapping;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.DeleteSurveyTriggerMapping
{
    public class DeleteSurveyTriggerMappingValidator: Validator<DeleteSurveyTriggerMappingCommand>
    {
        public DeleteSurveyTriggerMappingValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("Survey Trigger Mapping Id is required.")
                .GreaterThan(0).WithMessage("Survey Trigger Mapping Id must be greater than zero.");
        }
    }
}
