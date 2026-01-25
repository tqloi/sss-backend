using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.Roadmaps.GraphCreate;

namespace SSS.Web.Endpoints.Content.Roadmaps.GraphCreate;

public class CreateRoadmapGraphValidator : Validator<CreateRoadmapGraphCommand>
{
    public CreateRoadmapGraphValidator()
    {
        RuleFor(x => x.Roadmap.Title)
            .NotEmpty().WithMessage("Roadmap title is required.")
            .MaximumLength(300).WithMessage("Roadmap title cannot exceed 300 characters.");

        RuleFor(x => x.Roadmap.SubjectId)
            .GreaterThan(0).WithMessage("Valid SubjectId is required.");

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
