using FastEndpoints;
using FluentValidation;

namespace SSS.Web.Endpoints.UserManagement.CreateUser
{
    public sealed class CreateUserValidator : Validator<CreateUserRequest>
    {
        public CreateUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.FirstName)
                .NotEmpty();

            RuleFor(x => x.RoleName)
                .NotEmpty();
        }
    }
}
