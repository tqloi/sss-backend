using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.RoadmapEdges.Create;

namespace SSS.Web.Endpoints.Content.RoadmapEdges.Create
{
    public class CreateRoadmapEdgeValidator : Validator<CreateRoadmapEdgeCommand>
    {
        public CreateRoadmapEdgeValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");
            
            RuleFor(x => x.FromNodeId)
                .GreaterThan(0).WithMessage("FromNodeId must be greater than 0.");
            
            RuleFor(x => x.ToNodeId)
                .GreaterThan(0).WithMessage("ToNodeId must be greater than 0.");
            
            RuleFor(x => x.EdgeType)
                .IsInEnum().WithMessage("EdgeType must be Prerequisite, Recommended, or Next.");
        }
    }
}
