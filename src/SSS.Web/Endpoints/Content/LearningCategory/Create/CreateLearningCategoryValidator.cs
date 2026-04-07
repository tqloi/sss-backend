using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.LearningCategory.Create;

namespace SSS.Web.Endpoints.Content.LearningCategory.Create
{
    public class CreateLearningCategoryValidator : Validator<CreateLearningCategoryCommand>
    {
        public CreateLearningCategoryValidator()
        {
            RuleFor(x => x.Name)
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
        }
    }
}
