using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAttempts.GetQuizzesByQuizAttempt;

namespace SSS.Web.Endpoints.QuizAttempts.GetQuizzesByQuizAttempt
{
    public class GetQuizzesByQuizAttemptValidator : Validator<GetQuizzesByQuizAttemptQuery>
    {
        public GetQuizzesByQuizAttemptValidator()
        {
            RuleFor(x => x.AttemptId)
                .NotEmpty()
                .WithMessage("Attempt ID is required.")
                .GreaterThan(0)
                .WithMessage("Attempt ID must be a positive integer.");
        }
    }
}
