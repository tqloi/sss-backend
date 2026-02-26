using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.QuizAttempts.GetQuizAttemptById;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.QuizAttempts.GetQuizAttemptById
{
    public class GetQuizAttemptByIdValidator : Validator<GetQuizAttemptByIdQuery>
    {
        public GetQuizAttemptByIdValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty()
                .WithMessage("Quiz attempt ID is required.")
                .GreaterThan(0)
                .WithMessage("Quiz attempt ID must be a positive integer.");
        }
    }
}
