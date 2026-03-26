namespace SSS.WebApi.Endpoints.Notification.RegisterPushToken;

public sealed class RegisterPushTokenRequest
{
    public string DeviceToken { get; set; } = default!;
    public string DeviceType { get; set; } = default!;
}
