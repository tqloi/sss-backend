using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.Roadmap.Update;

namespace SSS.Web.Endpoints.Content.Roadmap.Update
{
    public class UpdateRoadmapValidator : Validator<UpdateRoadmapCommand>
    {
        public UpdateRoadmapValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            
            RuleFor(x => x.Title)
                .MaximumLength(300).WithMessage("Title cannot exceed 300 characters.")
                .When(x => x.Title is not null);
            
            RuleFor(x => x.Description)
                .MaximumLength(2000).WithMessage("Description cannot exceed 2000 characters.")
                .When(x => x.Description is not null);
        }
    }
}
