namespace SSS.WebApi.Endpoints.Notification.DeactivatePushToken;

public sealed class DeactivatePushTokenRequest
{
    public string DeviceToken { get; set; } = default!;
}
