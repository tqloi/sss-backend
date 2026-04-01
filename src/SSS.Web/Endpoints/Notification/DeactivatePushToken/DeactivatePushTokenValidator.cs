using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Notification.DeactivatePushToken;

public sealed class DeactivatePushTokenValidator : Validator<DeactivatePushTokenRequest>
{
    public DeactivatePushTokenValidator()
    {
        RuleFor(x => x.DeviceToken)
            .NotEmpty().WithMessage("DeviceToken is required.")
            .MaximumLength(512).WithMessage("DeviceToken must be 512 characters or fewer.");
    }
}
