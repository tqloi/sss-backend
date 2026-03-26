namespace SSS.WebApi.Endpoints.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadResponse
{
    public long Id { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}
