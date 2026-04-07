using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.NodeContents.Create;

namespace SSS.Web.Endpoints.Content.NodeContents.Create
{
    public class CreateNodeContentValidator : Validator<CreateNodeContentCommand>
    {
        public CreateNodeContentValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be greater than 0.");
            
            RuleFor(x => x.NodeId)
                .GreaterThan(0).WithMessage("NodeId must be greater than 0.");
            
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(400).WithMessage("Title cannot exceed 400 characters.");
            
            RuleFor(x => x.Url)
                .MaximumLength(2048).WithMessage("Url cannot exceed 2048 characters.");
            
            RuleFor(x => x.ContentType)
                .IsInEnum().WithMessage("ContentType must be valid.");
        }
    }
}
