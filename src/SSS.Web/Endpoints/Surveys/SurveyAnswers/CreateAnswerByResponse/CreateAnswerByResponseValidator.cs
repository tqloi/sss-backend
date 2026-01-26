using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyAnswers.CreateAnswerByResponse;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.CreateAnswerByResponse
{
    public class CreateAnswerByResponseValidator : Validator<CreateAnswerByResponseCommand>
    {
        public CreateAnswerByResponseValidator()
        {
            RuleFor(x => x.QuestionId)
                .GreaterThan(0)
                .WithMessage("QuestionId is required");

            RuleFor(x => x.OptionId)
                .GreaterThan(0)
                .When(x => x.OptionId.HasValue)
                .WithMessage("OptionId must be greater than 0");
        }
    }

   
}