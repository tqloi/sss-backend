using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestionOptions.UpdateQuizQuestionOption;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.QuizQuestionOptions.UpdateQuizQuestionOption
{
    public class UpdateQuizQuestionOptionValidator : Validator<UpdateQuizQuestionOptionCommand>
    {
        public UpdateQuizQuestionOptionValidator()
        {
            RuleFor(x => x.Id)
            .GreaterThan(0)
            .WithMessage("Id must be greater than 0.");

            // 2️⃣ Option phải tồn tại
            
            // 3️⃣ Validate ValueKey
            RuleFor(x => x.UpdateQuizQuestionOptionDto.ValueKey)
                .NotEmpty()
                .MaximumLength(10);

            // 4️⃣ Validate DisplayText
            RuleFor(x => x.UpdateQuizQuestionOptionDto.DisplayText)
                .NotEmpty()
                .MaximumLength(1000);

            // 5️⃣ Validate OrderNo
            RuleFor(x => x.UpdateQuizQuestionOptionDto.OrderNo)
                .GreaterThanOrEqualTo(0);

            // 6️⃣ Score logic
            When(x => x.UpdateQuizQuestionOptionDto.IsCorrect, () =>
            {
                RuleFor(x => x.UpdateQuizQuestionOptionDto.ScoreValue)
                    .NotNull()
                    .GreaterThan(0)
                    .WithMessage("Correct option must have positive ScoreValue.");
            });

            When(x => x.UpdateQuizQuestionOptionDto.ScoreValue.HasValue, () =>
            {
                RuleFor(x => x.UpdateQuizQuestionOptionDto.ScoreValue)
                    .GreaterThanOrEqualTo(0)
                    .WithMessage("ScoreValue cannot be negative.");
            });

           

        }
    }
}
