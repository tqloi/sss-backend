namespace SSS.Application.Features.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadResult
{
    public bool Found { get; set; }
    public long Id { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
}
