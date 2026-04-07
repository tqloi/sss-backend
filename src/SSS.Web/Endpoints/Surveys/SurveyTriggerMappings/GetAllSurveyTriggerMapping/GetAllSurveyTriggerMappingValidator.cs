using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyTriggerMappings.GetAllSurveyTriggerMapping;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyTriggerMappings.GetAllSurveyTriggerMapping
{
    public class GetAllSurveyTriggerMappingValidator: Validator<GetAllSurveyTriggerMappingQuery>
    {
        public GetAllSurveyTriggerMappingValidator()
        {
            RuleFor(x => x.PageIndex)
               .GreaterThan(0).WithMessage("PageIndex must be greater than 0.");

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
        }
    }
}
