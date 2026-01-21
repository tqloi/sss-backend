using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.GetAllQuizzesByStudyPlanModuleId;
using System.ComponentModel.DataAnnotations;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzesByStudyPlanModuleId
{
    public class GetAllQuizzesByStudyPlanModuleIdValidator : Validator<GetAllQuizzesByStudyPlanModuleIdRequest>
    {
        public GetAllQuizzesByStudyPlanModuleIdValidator()
        {
            RuleFor(x => x.StudyPlanModuleId)
                .GreaterThan(0)
                .WithMessage("");
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("PageIndex must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
