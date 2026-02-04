using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.GetSurveyTriggerMappingById;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetSurveyTriggerMappingById
{
    public class GetSurveyTriggerMappingByIdValidator : Validator<GetSurveyTriggerMappingByIdQuery>
    {
        public GetSurveyTriggerMappingByIdValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required")
                .Must(id => id > 0).WithMessage("Id must be a positive integer");
        }
    }
}
