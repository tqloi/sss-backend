using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Surveys.SurveyAnswers.UpdateAnswer;

namespace SSS.Web.Endpoints.Surveys.SurveyAnswers.UpdateAnswer
{
    public class UpdateAnswerValidator : Validator<UpdateAnswerCommand>
    {
        public UpdateAnswerValidator()
        {
            RuleFor(x => x.OptionId)
                .GreaterThan(0)
                .When(x => x.OptionId.HasValue)
                .WithMessage("OptionId must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.OptionId.HasValue || x.NumberValue.HasValue || !string.IsNullOrWhiteSpace(x.TextValue))
                .WithMessage("At least one value (OptionId, NumberValue, or TextValue) must be provided");
        }
    }
}