using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Notification.SendTestNotification;

public sealed class SendTestNotificationValidator : Validator<SendTestNotificationRequest>
{
    public SendTestNotificationValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(300).WithMessage("Title must be 300 characters or fewer.");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required.");
    }
}
