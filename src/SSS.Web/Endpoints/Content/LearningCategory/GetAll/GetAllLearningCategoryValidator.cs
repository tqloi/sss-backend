using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.LearningCategory.GetAll;

namespace SSS.Web.Endpoints.Content.LearningCategory.GetAll
{
    public class GetAllLearningCategoryValidator : Validator<GetAllLearningCategoryQuery>
    {
        public GetAllLearningCategoryValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("PageIndex must be at least 1.");
            
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
        }
    }
}
