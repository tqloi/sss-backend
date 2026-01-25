using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.DeleteSurveyQuestionOption;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.DeleteSurveyQuestionOption
{
    public class DeleteSurveyQuestionOptionValidator: Validator<DeleteSurveyQuestionOptionCommand>
    {

        public DeleteSurveyQuestionOptionValidator() 
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
