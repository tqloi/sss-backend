namespace SSS.WebApi.Endpoints.Notification.DeactivatePushToken;

public sealed class DeactivatePushTokenResponse
{
    public bool Deactivated { get; set; }
    public string UserId { get; set; } = default!;
    public string DeviceToken { get; set; } = default!;
    public DateTime LastUpdated { get; set; }
}
