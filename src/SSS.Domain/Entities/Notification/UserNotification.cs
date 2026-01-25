using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Tracking;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Notification;

public class UserNotification
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string Content { get; set; } = null!;
    public NotificationType Type { get; set; } = NotificationType.System;
    public NotificationRelatedType? RelatedType { get; set; }
    public long? RelatedId { get; set; }
    public string? RelatedSessionId { get; set; }
    public bool IsRead { get; set; }
    public DateTime? ReadAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public virtual User User { get; set; } = null!;
    public virtual StudySession? RelatedSession { get; set; }
}