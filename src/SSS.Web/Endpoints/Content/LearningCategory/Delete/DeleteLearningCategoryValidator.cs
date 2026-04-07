using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.LearningCategory.Delete;

namespace SSS.Web.Endpoints.Content.LearningCategory.Delete
{
    public class DeleteLearningCategoryValidator : Validator<DeleteLearningCategoryCommand>
    {
        public DeleteLearningCategoryValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
        }
    }
}
