using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.Surveys.EditSurvey;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.Surveys.EditSurvey
{
    public class EditSurveyValidator: Validator<EditSurveyCommand>
    {
        public EditSurveyValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
            
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required").MaximumLength(100).WithMessage("Prompt must not exceed 100 words");
            
        }
    }
}
