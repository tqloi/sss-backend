using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.Roadmaps.GraphSync;

namespace SSS.Web.Endpoints.Content.Roadmaps.GraphSync;

public class SyncRoadmapGraphValidator : Validator<SyncRoadmapGraphCommand>
{
    public SyncRoadmapGraphValidator()
    {
        RuleFor(x => x.RoadmapId)
            .GreaterThan(0).WithMessage("Valid RoadmapId is required.");

        When(x => x.Roadmap != null, () =>
        {
            RuleFor(x => x.Roadmap!.Title)
                .MaximumLength(300).When(x => !string.IsNullOrEmpty(x.Roadmap!.Title))
                .WithMessage("Roadmap title cannot exceed 300 characters.");
        });

        RuleForEach(x => x.Nodes).ChildRules(node =>
        {
            node.RuleFor(n => n.Title)
                .NotEmpty().WithMessage("Node title is required.");

            node.RuleFor(n => n.ClientId)
                .NotEmpty().When(n => !n.Id.HasValue)
                .WithMessage("ClientId is required when Id is not provided.");
        });

        RuleForEach(x => x.Contents).ChildRules(content =>
        {
            content.RuleFor(c => c.Title)
                .NotEmpty().WithMessage("Content title is required.");

            content.RuleFor(c => c.OrderNo)
                .GreaterThanOrEqualTo(0).WithMessage("OrderNo must be >= 0.");

            content.RuleFor(c => c.ClientId)
                .NotEmpty().When(c => !c.Id.HasValue)
                .WithMessage("ClientId is required when Id is not provided.");

            content.RuleFor(c => c)
                .Must(c => c.NodeId.HasValue || !string.IsNullOrEmpty(c.NodeClientId))
                .WithMessage("Content must reference a node via NodeId or NodeClientId.");
        });

        RuleForEach(x => x.Edges).ChildRules(edge =>
        {
            edge.RuleFor(e => e)
                .Must(e => e.FromNodeId.HasValue || !string.IsNullOrEmpty(e.FromNodeClientId))
                .WithMessage("Edge must reference fromNode via FromNodeId or FromNodeClientId.");

            edge.RuleFor(e => e)
                .Must(e => e.ToNodeId.HasValue || !string.IsNullOrEmpty(e.ToNodeClientId))
                .WithMessage("Edge must reference toNode via ToNodeId or ToNodeClientId.");
        });
    }
}
