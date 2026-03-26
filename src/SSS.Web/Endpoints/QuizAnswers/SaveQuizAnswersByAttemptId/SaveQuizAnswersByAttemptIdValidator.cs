using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAnswers.SaveQuizAnswersByAttemptId;

namespace SSS.Web.Endpoints.QuizAnswers.SaveQuizAnswersByAttemptId
{
    public class SaveQuizAnswersByAttemptIdValidator : Validator<SaveQuizAnswersByAttemptIdCommand>
    {
        public SaveQuizAnswersByAttemptIdValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty()
                .WithMessage("Attempt ID is required.")
                .GreaterThan(0)
                .WithMessage("Attempt ID must be a positive integer.");

            RuleFor(x => x.QuizAnswers)
                .NotEmpty()
                .WithMessage("At least one quiz answer is required.");

            RuleForEach(x => x.QuizAnswers)
                .ChildRules(answer =>
                {
                    answer.RuleFor(a => a.Id)
                        .GreaterThan(0)
                        .WithMessage("Quiz answer ID must be a positive integer.");
                });
        }
    }
}
