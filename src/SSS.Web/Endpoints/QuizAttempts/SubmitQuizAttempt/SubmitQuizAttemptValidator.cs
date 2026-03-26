using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAttempts.SubmitQuizAttemp;

namespace SSS.Web.Endpoints.QuizAttempts.SubmitQuizAttempt
{
    public class SubmitQuizAttemptValidator : Validator<SubmitQuizAttemptCommand>
    {
        public SubmitQuizAttemptValidator()
        {
            RuleFor(x => x.SubmitQuizAttempt.Answers)
                .NotEmpty()
                .WithMessage("At least one answer is required.");

            RuleForEach(x => x.SubmitQuizAttempt.Answers)
                .ChildRules(answer =>
                {
                    answer.RuleFor(a => a.QuestionId)
                        .NotEmpty()
                        .WithMessage("Question ID is required.")
                        .GreaterThan(0)
                        .WithMessage("Question ID must be a positive integer.");
                });
        }
    }
}
