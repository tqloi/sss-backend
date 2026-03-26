using FastEndpoints;
using FluentValidation;
using SSS.Application.Features.UserManagement.GetAllUsers;

namespace SSS.Web.Endpoints.UserManagement.GetAllUsers
{
    public sealed class GetAllUsersValidator : Validator<GetAllUsersQuery>
    {
        public GetAllUsersValidator()
        {
            RuleFor(x => x.PageIndex)
                .GreaterThanOrEqualTo(1)
                .WithMessage("PageIndex must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0)
                .WithMessage("PageSize must be greater than 0.")
                .LessThanOrEqualTo(100)
                .WithMessage("PageSize cannot exceed 100.");

            RuleFor(x => x.Name)
                .MaximumLength(100)
                .When(x => !string.IsNullOrWhiteSpace(x.Name));

            RuleFor(x => x.Role)
                .MaximumLength(50)
                .When(x => !string.IsNullOrWhiteSpace(x.Role));
        }
    }
}
