using SSS.Domain.Enums;

namespace SSS.Application.Abstractions.Services;

public interface INotificationService
{
    Task<long> CreateAndDispatchAsync(
        string userId,
        string title,
        string content,
        NotificationType type = NotificationType.System,
        NotificationRelatedType? relatedType = null,
        long? relatedId = null,
        string? relatedSessionId = null,
        CancellationToken ct = default);
}
