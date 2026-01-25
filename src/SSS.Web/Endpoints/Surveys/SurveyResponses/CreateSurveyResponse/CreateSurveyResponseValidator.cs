using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyResponses.CreateSurveyResponse
{
    public class CreateSurveyResponseValidator: Validator<CreateSurveyResponseCommand>
    {
        public CreateSurveyResponseValidator() 
        {
            RuleFor(x => x.SurveyId).GreaterThan(0);
            
        }
    }
}
