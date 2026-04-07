using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.GetSurveyQuestionOptionById
{
    public class GetSurveyQuestionOptionByIdValidator : Validator<GetSurveyQuestionOptionByIdQuery>
    {
        public GetSurveyQuestionOptionByIdValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");
        }
    
    }
}
