using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.Roadmap.GetById;

namespace SSS.Web.Endpoints.Content.Roadmap.GetById
{
    public class GetRoadmapByIdValidator : Validator<GetRoadmapByIdQuery>
    {
        public GetRoadmapByIdValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");
        }
    }
}
