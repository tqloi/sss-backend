using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizQuestions.CreateQuizQuestions;

namespace SSS.Web.Endpoints.QuizQuestions.CreateQuizQuestions
{
    public class CreateQuizQuestionsValidator : Validator<CreateQuizQuestionsCommand>
    {
        public CreateQuizQuestionsValidator()
        {
            RuleFor(x => x.CreateQuizQuestionDtos)
                .NotEmpty().WithMessage("At least one quiz question is required.");

            RuleForEach(x => x.CreateQuizQuestionDtos).ChildRules(question =>
            {
                question.RuleFor(q => q.QuizId)
                    .GreaterThan(0).WithMessage("QuizId must be greater than 0.");

                question.RuleFor(q => q.Level)
                    .NotEmpty().WithMessage("Level is required.")
                    .MaximumLength(50).WithMessage("Level cannot exceed 50 characters.");

                question.RuleFor(q => q.QuestionKey)
                    .NotEmpty().WithMessage("QuestionKey is required.")
                    .MaximumLength(100).WithMessage("QuestionKey cannot exceed 100 characters.");

                question.RuleFor(q => q.Prompt)
                    .NotEmpty().WithMessage("Prompt is required.");

                question.RuleFor(q => q.ScoreWeight)
                    .GreaterThanOrEqualTo(0).WithMessage("ScoreWeight must be greater than or equal to 0.");

                question.RuleFor(q => q.OrderNo)
                    .GreaterThanOrEqualTo(0).WithMessage("OrderNo must be greater than or equal to 0.");

                question.RuleForEach(q => q.Options).ChildRules(option =>
                {
                    option.RuleFor(o => o.ValueKey)
                        .NotEmpty().WithMessage("ValueKey is required.")
                        .MaximumLength(100).WithMessage("ValueKey cannot exceed 100 characters.");

                    option.RuleFor(o => o.DisplayText)
                        .NotEmpty().WithMessage("DisplayText is required.")
                        .MaximumLength(300).WithMessage("DisplayText cannot exceed 300 characters.");

                    option.RuleFor(o => o.OrderNo)
                        .GreaterThanOrEqualTo(0).WithMessage("Option OrderNo must be greater than or equal to 0.");

                    option.When(o => o.ScoreValue.HasValue, () =>
                    {
                        option.RuleFor(o => o.ScoreValue!.Value)
                            .GreaterThanOrEqualTo(0).WithMessage("ScoreValue cannot be negative.");
                    });
                });
            });
        }
    }
}
