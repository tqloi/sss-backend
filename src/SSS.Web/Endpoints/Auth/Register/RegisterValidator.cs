using FastEndpoints;
using FluentValidation;
using SSS.WebApi.Endpoints.Auth.Register;

namespace SSS.WebApi.Endpoints.Auth.ResetPassword
{
    public class RegisterValidator : Validator<RegisterRequest>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password)
                .WithMessage("Passwords do not match.");

            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);

            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
