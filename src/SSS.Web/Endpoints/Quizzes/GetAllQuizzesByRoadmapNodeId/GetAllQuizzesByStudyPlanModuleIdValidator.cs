using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Quizzes.GetAllQuizzesByRoadmapNodeId;

namespace SSS.Web.Endpoints.Quizzes.GetAllQuizzesByRoadmapNodeId
{
    public class GetAllQuizzesByRoadmapNodeIdValidator : Validator<GetAllQuizzesByRoadmapNodeIdQuery>
    {
        public GetAllQuizzesByRoadmapNodeIdValidator()
        {
            RuleFor(x => x.RoadmapNodeId)
                .GreaterThan(0)
                .WithMessage("RoadmapNodeId must be greater than 0");
            RuleFor(x => x.PageIndex)
                .GreaterThan(0)
                .WithMessage("PageIndex must be greater than 0.");
            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.");
        }
    }
}
