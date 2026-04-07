namespace SSS.Application.Features.Notification.RegisterPushToken;

public sealed class RegisterPushTokenResult
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
    public string DeviceToken { get; set; } = default!;
    public string DeviceType { get; set; } = default!;
    public bool IsActive { get; set; }
    public DateTime LastUpdated { get; set; }
}
