using MediatR;

namespace SSS.Application.Features.Notification.RegisterPushToken;

public sealed class RegisterPushTokenCommand : IRequest<RegisterPushTokenResult>
{
    public string UserId { get; set; } = default!;
    public string DeviceToken { get; set; } = default!;
    public string DeviceType { get; set; } = default!;
}
