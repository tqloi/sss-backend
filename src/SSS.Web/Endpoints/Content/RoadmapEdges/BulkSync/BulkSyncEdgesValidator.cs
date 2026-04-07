using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.RoadmapEdges.BulkSync;

namespace SSS.Web.Endpoints.Content.RoadmapEdges.BulkSync
{
    public class BulkSyncEdgesValidator : Validator<BulkSyncEdgesCommand>
    {
        public BulkSyncEdgesValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");
            
            RuleFor(x => x.Edges)
                .NotNull().WithMessage("Edges list is required.");
            
            RuleForEach(x => x.Edges).ChildRules(edge =>
            {
                edge.RuleFor(e => e.FromNodeId)
                    .GreaterThan(0).WithMessage("FromNodeId must be greater than 0.");
                
                edge.RuleFor(e => e.ToNodeId)
                    .GreaterThan(0).WithMessage("ToNodeId must be greater than 0.");
                
                edge.RuleFor(e => e.EdgeType)
                    .IsInEnum().WithMessage("EdgeType must be valid.");
            });
        }
    }
}
