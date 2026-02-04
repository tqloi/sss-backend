using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping
{
    public class CreateSurveyTriggerMappingValidator: Validator<CreateSurveyTriggerMappingCommand>
    {
        public CreateSurveyTriggerMappingValidator()
        {
            RuleFor(x => x.SurveyId)
                .NotEmpty().WithMessage("Survey Id is required.")
                .GreaterThan(0).WithMessage("Survey Id must be greater than zero.");
            RuleFor(x => x.TriggerType)
                .NotEmpty().WithMessage("Trigger Event is required.");
               
           
        }
    }
}
