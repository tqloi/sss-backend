
using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.Surveys.CreateSurvey;

namespace SSS.Web.Endpoints.Surveys.Surveys.CreateSurvey
{
    public class CreateSurveyValidator : Validator<CreateSurveyCommand>
    {
        public CreateSurveyValidator() 
        {
            RuleFor(x => x.Code).NotEmpty().WithMessage("Code is required").MaximumLength(20).WithMessage("Code must not exceed 20 words");
            RuleFor(x => x.Title).MaximumLength(1000).WithMessage("Title must not exceed 1000 words");
        }
        
    }
}
