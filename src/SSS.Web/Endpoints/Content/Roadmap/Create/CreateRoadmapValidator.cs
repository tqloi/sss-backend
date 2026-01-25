using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.Roadmap.Create;

namespace SSS.Web.Endpoints.Content.Roadmap.Create
{
    public class CreateRoadmapValidator : Validator<CreateRoadmapCommand>
    {
        public CreateRoadmapValidator()
        {
            RuleFor(x => x.SubjectId)
                .GreaterThan(0).WithMessage("SubjectId must be greater than 0.");
            
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.");
            
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.");
        }
    }
}
