using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.UpdateQuizNode;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Quizzes.UpdateQuiz
{
    public class UpdateQuizValidator : Validator<UpdateQuizRequest>
    {
        public UpdateQuizValidator() 
        {
            RuleFor(x => x.UpdateQuizNodeDto.TotalScore)
                .GreaterThanOrEqualTo(0).WithMessage("Must be >= 0");
            RuleFor(x => x.UpdateQuizNodeDto.TotalScore).GreaterThan(0);
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");
        }
    }
}
