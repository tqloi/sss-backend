
using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestionOptions.CreateQuizQuestionOption;

namespace SSS.Web.Endpoints.QuizQuestionOptions.CreateQuizQuestionOption
{
    public class CreateQuizQuestionOptionValidate : Validator<CreateQuizQuestionOptionCommand>
    {

        public CreateQuizQuestionOptionValidate()
        {
            RuleFor(x => x.CreateQuizQuestionOptionDto.QuestionId)
            .GreaterThan(0)
            .WithMessage("QuestionId must be greater than 0.");

            RuleFor(x => x.CreateQuizQuestionOptionDto.ValueKey)
                .NotEmpty()
                .MaximumLength(10)
                .WithMessage("ValueKey is required and must be <= 10 characters.");

            RuleFor(x => x.CreateQuizQuestionOptionDto.DisplayText)
                .NotEmpty()
                .MaximumLength(1000)
                .WithMessage("DisplayText is required.");

            RuleFor(x => x.CreateQuizQuestionOptionDto.OrderNo)
                .GreaterThanOrEqualTo(0)
                .WithMessage("OrderNo must be >= 0.");

            // Nếu IsCorrect = true thì ScoreValue phải có
            When(x => x.CreateQuizQuestionOptionDto.IsCorrect, () =>
            {
                RuleFor(x => x.CreateQuizQuestionOptionDto.ScoreValue)
                    .NotNull()
                    .GreaterThan(0)
                    .WithMessage("Correct option must have positive ScoreValue.");
            });

            // Nếu không correct → ScoreValue có thể null hoặc >= 0
            When(x => x.CreateQuizQuestionOptionDto.ScoreValue.HasValue, () =>
            {
                RuleFor(x => x.CreateQuizQuestionOptionDto.ScoreValue)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("ScoreValue cannot be negative.");
            });
        }
    }
}
