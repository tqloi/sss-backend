using MediatR;

namespace SSS.Application.Features.Notification.DeactivatePushToken;

public sealed class DeactivatePushTokenCommand : IRequest<DeactivatePushTokenResult>
{
    public string UserId { get; set; } = default!;
    public string DeviceToken { get; set; } = default!;
}
