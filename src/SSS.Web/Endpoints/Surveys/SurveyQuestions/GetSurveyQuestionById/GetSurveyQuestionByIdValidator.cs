using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestions.GetSurveyQuestionById;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.GetSurveyQuestionById
{
    public class GetSurveyQuestionByIdValidator : Validator<GetSurveyQuestionByIdQuery>
    {
        public GetSurveyQuestionByIdValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }


    }
}
