using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.Reviews.CreateReview
{
    public class CreateReviewValidator : Validator<CreateReviewRequest>
    {
        public CreateReviewValidator()
        {
            RuleFor(x => x.RoadmapId)
                .GreaterThan(0).WithMessage("RoadmapId must be a valid positive number.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(2000).WithMessage("Comment must not exceed 2000 characters.")
                .When(x => x.Comment != null);
        }
    }
}
