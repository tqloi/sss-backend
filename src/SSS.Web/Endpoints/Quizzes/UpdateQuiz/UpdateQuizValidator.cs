using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.UpdateQuizNode;

namespace SSS.Web.Endpoints.Quizzes.UpdateQuiz
{
    public class UpdateQuizValidator : Validator<UpdateQuizCommand>
    {
        public UpdateQuizValidator() 
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.UpdateQuizNodeDto.TotalScore)
                .GreaterThanOrEqualTo(0).WithMessage("TotalScore must be greater than or equal to 0.");

            RuleFor(x => x.UpdateQuizNodeDto.Level)
                .NotEmpty().WithMessage("Level is required.")
                .MaximumLength(50).WithMessage("Level cannot exceed 50 characters.");

            RuleFor(x => x.UpdateQuizNodeDto.PassingScore)
                .GreaterThanOrEqualTo(0).WithMessage("PassingScore must be greater than or equal to 0.");

            RuleFor(x => x.UpdateQuizNodeDto)
                .Must(x => !x.TotalScore.HasValue || x.PassingScore <= x.TotalScore.Value)
                .WithMessage("PassingScore cannot be greater than TotalScore.");
        }
    }
}
