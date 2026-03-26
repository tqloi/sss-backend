using SSS.Domain.Enums;

namespace SSS.WebApi.Endpoints.Notification.SendTestNotification;

public sealed class SendTestNotificationRequest
{
    public string Title { get; set; } = "Test notification";
    public string Content { get; set; } = "This is a realtime notification test.";
    public NotificationType Type { get; set; } = NotificationType.System;
}
