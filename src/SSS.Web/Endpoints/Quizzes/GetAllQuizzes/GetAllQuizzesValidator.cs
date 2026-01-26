using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.GetAllQuizNode;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzes
{
    public class GetAllQuizzesValidator : Validator<GetAllQuizzesRequest>
    {
        public GetAllQuizzesValidator() 
        {
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("PageIndex must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
