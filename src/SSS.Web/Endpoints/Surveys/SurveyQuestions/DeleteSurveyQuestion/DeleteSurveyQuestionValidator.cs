using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestions.DeleteSurveyQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.DeleteSurveyQuestion
{
    public class DeleteSurveyQuestionValidator : Validator<DeleteSurveyQuestionCommand>
    {
        public DeleteSurveyQuestionValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
