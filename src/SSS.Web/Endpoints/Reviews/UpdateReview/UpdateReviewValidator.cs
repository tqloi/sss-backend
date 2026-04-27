using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.Reviews.UpdateReview
{
    public class UpdateReviewValidator : Validator<UpdateReviewRequest>
    {
        public UpdateReviewValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Review Id must be a valid positive number.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Comment)
                .MaximumLength(2000).WithMessage("Comment must not exceed 2000 characters.")
                .When(x => x.Comment != null);
        }
    }
}
