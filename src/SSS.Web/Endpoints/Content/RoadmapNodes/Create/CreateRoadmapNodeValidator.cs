using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.RoadmapNodes.Create;

namespace SSS.Web.Endpoints.Content.RoadmapNodes.Create
{
    public class CreateRoadmapNodeValidator : Validator<CreateRoadmapNodeCommand>
    {
        public CreateRoadmapNodeValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");
            
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.");
            
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");
        }
    }
}
