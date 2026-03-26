using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Notification.RegisterPushToken;

public sealed class RegisterPushTokenValidator : Validator<RegisterPushTokenRequest>
{
    public RegisterPushTokenValidator()
    {
        RuleFor(x => x.DeviceToken)
            .NotEmpty().WithMessage("DeviceToken is required.")
            .MaximumLength(512).WithMessage("DeviceToken must be 512 characters or fewer.");

        RuleFor(x => x.DeviceType)
            .NotEmpty().WithMessage("DeviceType is required.")
            .MaximumLength(20).WithMessage("DeviceType must be 20 characters or fewer.");
    }
}
