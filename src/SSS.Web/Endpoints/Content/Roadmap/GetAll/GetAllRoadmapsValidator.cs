using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Content.Roadmap.GetAll;

namespace SSS.Web.Endpoints.Content.Roadmap.GetAll
{
    public class GetAllRoadmapsValidator : Validator<GetAllRoadmapsQuery>
    {
        public GetAllRoadmapsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1).WithMessage("PageIndex must be at least 1.");
            
            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100.");
            
            RuleFor(x => x.SubjectId)
                .GreaterThan(0).WithMessage("SubjectId must be greater than 0.")
                .When(x => x.SubjectId.HasValue);
        }
    }
}
