using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.RoadmapNodes.GetPreviousNodeContents;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.GetPreviousNodeContents
{
    public class GetPreviousNodeContentsValidator : Validator<GetPreviousNodeContentsQuery>
    {
        public GetPreviousNodeContentsValidator()
        {
            RuleFor(x => x.StudyPlanId)
                .GreaterThan(0).WithMessage("StudyPlanId must be greater than 0.");

            RuleFor(x => x.RoadmapNodeId)
                .GreaterThan(0).WithMessage("RoadmapNodeId must be greater than 0.");
        }
    }
}
