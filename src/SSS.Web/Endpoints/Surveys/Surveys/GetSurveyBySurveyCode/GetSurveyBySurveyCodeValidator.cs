using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.Surveys.GetSurveyBySurveyCode;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.Surveys.GetSurveyBySurveyCode
{
    public class GetSurveyBySurveyCodeValidator: Validator<GetSurveyBySurveyCodeQuery>
    {
        public GetSurveyBySurveyCodeValidator()
        {
            RuleFor(x => x.SurveyCode)
                .NotEmpty().WithMessage("Survey code is required")
                .MaximumLength(1000).WithMessage("Survey code must not exceed 1000 characters");
        }
    }
}
