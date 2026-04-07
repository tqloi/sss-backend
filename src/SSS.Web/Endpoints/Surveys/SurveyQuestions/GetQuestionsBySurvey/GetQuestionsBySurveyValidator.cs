using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestions.GetQuestionsBySurvey;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.GetQuestionsBySurvey
{
    public class GetQuestionsBySurveyValidator : Validator<GetQuestionsBySurveyQuery>
    {
        public GetQuestionsBySurveyValidator() 
        {
            RuleFor(x => x.surveyId)
                .GreaterThan(0)
                .WithMessage("SurveyId must be greater than 0.");
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("PageIndex must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
