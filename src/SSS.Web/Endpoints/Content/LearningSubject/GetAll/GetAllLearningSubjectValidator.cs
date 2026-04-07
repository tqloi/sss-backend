using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.LearningSubject.GetAll;

namespace SSS.Web.Endpoints.Content.LearningSubject.GetAll
{
    public class GetAllLearningSubjectValidator : Validator<GetAllLearningSubjectQuery>
    {
        public GetAllLearningSubjectValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("PageIndex must be at least 1.");
            
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
            
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("CategoryId must be greater than 0.")
                .When(x => x.CategoryId.HasValue);
        }
    }
}
