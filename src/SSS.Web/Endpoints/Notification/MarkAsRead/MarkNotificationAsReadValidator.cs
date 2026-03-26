using FastEndpoints;
using FluentValidation;

namespace SSS.WebApi.Endpoints.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadValidator : Validator<MarkNotificationAsReadRequest>
{
    public MarkNotificationAsReadValidator()
    {
        RuleFor(x => x.Id)
            .GreaterThan(0).WithMessage("Id must be greater than 0.");
    }
}
