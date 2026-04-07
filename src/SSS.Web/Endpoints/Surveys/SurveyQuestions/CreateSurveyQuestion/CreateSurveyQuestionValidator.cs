using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyQuestions.CreateSurveyQuestion;

namespace SSS.Web.Endpoints.Surveys.SurveyQuestions.CreateSurveyQuestion
{
    public class CreateSurveyQuestionValidator : Validator<CreateSurveyQuestionCommand>
    {
        public CreateSurveyQuestionValidator()
        {
            RuleFor(x => x.SurveyId).GreaterThan(0).WithMessage("SurveyId must be greater than 0");
            RuleFor(x => x.Prompt).NotEmpty().WithMessage("Prompt is required").MaximumLength(1000).WithMessage("Prompt must not exceed 1000 words");
            RuleFor(x => x.QuestionKey).NotEmpty().WithMessage("QuestionKey is required").MaximumLength(1000).WithMessage("QuestionKey must not exceed 1000 words");

        }

    }
}
