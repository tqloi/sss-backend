using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Users.ChangePassword;

public sealed class ChangePasswordValidator : Validator<ChangePasswordRequest>
{
    public ChangePasswordValidator()
    {
        RuleFor(x => x.CurrentPassword)
            .NotEmpty().WithMessage("Current password is required");

        RuleFor(x => x.NewPassword)
            .NotEmpty().WithMessage("New password is required")
            .MinimumLength(8).WithMessage("New password must be at least 8 characters")
            .Matches("[A-Z]").WithMessage("New password must contain at least one uppercase letter")
            .Matches("[a-z]").WithMessage("New password must contain at least one lowercase letter")
            .Matches("[0-9]").WithMessage("New password must contain at least one digit")
            .Matches("[^a-zA-Z0-9]").WithMessage("New password must contain at least one special character");

        RuleFor(x => x.ConfirmNewPassword)
            .NotEmpty().WithMessage("Confirm password is required")
            .Equal(x => x.NewPassword).WithMessage("Passwords do not match");
    }
}
