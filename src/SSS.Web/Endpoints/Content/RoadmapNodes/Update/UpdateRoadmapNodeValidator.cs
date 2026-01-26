using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.RoadmapNodes.Update;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.Update
{
    public class UpdateRoadmapNodeValidator : Validator<UpdateRoadmapNodeCommand>
    {
        public UpdateRoadmapNodeValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");
            
            RuleFor(x => x.NodeId)
                .GreaterThan(0).WithMessage("NodeId must be greater than 0.");
            
            RuleFor(x => x.Title)
                .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.")
                .When(x => x.Title is not null);
            
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
                .When(x => x.Description is not null);
        }
    }
}
