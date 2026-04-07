using SSS.Domain.Enums;

namespace SSS.Application.Features.Notification.GetMyNotifications;

public sealed class GetMyNotificationsResult
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public int UnreadCount { get; set; }
    public List<NotificationItem> Items { get; set; } = new();

    public sealed class NotificationItem
    {
        public long Id { get; set; }
        public string Title { get; set; } = default!;
        public string Content { get; set; } = default!;
        public NotificationType Type { get; set; }
        public NotificationRelatedType? RelatedType { get; set; }
        public long? RelatedId { get; set; }
        public string? RelatedSessionId { get; set; }
        public bool IsRead { get; set; }
        public DateTime? ReadAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
