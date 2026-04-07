using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestionOptions.CreateSurveyQuestionOption
{
    public class CreateSurveyQuestionOptionValidator : Validator<CreateSurveyQuestionOptionCommand>
    {
        public CreateSurveyQuestionOptionValidator() 
        {
            RuleFor(x => x.QuestionId).GreaterThan(0).WithMessage("QuestionId must be greater than 0");
            RuleFor(x => x.DisplayText).MaximumLength(1000).WithMessage("DisplayText must not exceed 1000 words");
            RuleFor(x => x.ValueKey).MaximumLength(1000).WithMessage("ValueKey must not exceed 1000 words");
        }
    }
}
