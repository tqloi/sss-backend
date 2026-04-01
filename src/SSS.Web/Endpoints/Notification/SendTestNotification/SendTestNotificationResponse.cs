namespace SSS.WebApi.Endpoints.Notification.SendTestNotification;

public sealed class SendTestNotificationResponse
{
    public long NotificationId { get; set; }
    public string Message { get; set; } = default!;
}
