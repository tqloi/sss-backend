using SSS.Domain.Enums;

namespace SSS.Application.Features.Notification.Common;

public sealed class RealtimeNotificationDto
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
    public string Title { get; set; } = default!;
    public string Content { get; set; } = default!;
    public NotificationType Type { get; set; }
    public NotificationRelatedType? RelatedType { get; set; }
    public long? RelatedId { get; set; }
    public string? RelatedSessionId { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
