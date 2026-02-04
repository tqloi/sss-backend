using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyFieldSemantics.GetSurveyFieldSemanticByQuestion;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Surveys.SurveyFieldSemantics.GetSurveyFieldSemanticByQuestion
{
    public class GetSurveyFieldSemanticByQuestionValidator: Validator<GetSurveyFieldSemanticByQuestionQuery>

    {
        public GetSurveyFieldSemanticByQuestionValidator()
        {
            RuleFor(x => x.QuestionId)
                .NotEmpty().WithMessage("Question Id is required.")
                .GreaterThan(0).WithMessage("Question Id must be greater than zero.");
        }
    }
}
