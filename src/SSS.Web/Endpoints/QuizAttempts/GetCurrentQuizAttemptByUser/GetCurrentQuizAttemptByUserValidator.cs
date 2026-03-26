using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAttempts.GetCurrentQuizAttemptByUser;

namespace SSS.Web.Endpoints.QuizAttempts.GetCurrentQuizAttemptByUser
{
    public class GetCurrentQuizAttemptByUserValidator : Validator<GetCurrentQuizAttemptByUserQuery>
    {
        public GetCurrentQuizAttemptByUserValidator()
        {
            RuleFor(x => x.ModuleId)
                .NotEmpty()
                .WithMessage("Module ID is required.")
                .GreaterThan(0)
                .WithMessage("Module ID must be a positive integer.");
        }
    }
}
