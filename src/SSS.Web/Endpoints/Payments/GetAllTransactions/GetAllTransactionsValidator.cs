using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.Payments.GetAllTransactions;

namespace SSS.Web.Endpoints.Payments.GetAllTransactions
{
    public sealed class GetAllTransactionsValidator : Validator<GetAllTransactionsQuery>
    {
        public GetAllTransactionsValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageIndex must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize cannot exceed 100.");
        }
    }
}
