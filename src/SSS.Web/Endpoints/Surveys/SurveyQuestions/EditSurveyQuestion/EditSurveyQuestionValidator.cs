using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestions.EditSurveyQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.EditSurveyQuestion
{
    public class EditSurveyQuestionValidator : Validator<EditSurveyQuestionCommand>
    {
        public EditSurveyQuestionValidator()
        {
            RuleFor(x => x.Id).GreaterThan(0).WithMessage("Id must be greater than 0");
            RuleFor(x => x.SurveyId).GreaterThan(0).WithMessage("SurveyId must be greater than 0");
            RuleFor(x => x.Prompt).NotEmpty().WithMessage("Prompt is required").MaximumLength(1000).WithMessage("Prompt must not exceed 1000 words");
        }
    }
}
