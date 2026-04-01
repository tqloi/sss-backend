using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.Communication.Push;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Features.Notification.Common;
using SSS.Domain.Entities.Notification;
using SSS.Domain.Enums;
using SSS.Infrastructure.Realtime;

namespace SSS.Infrastructure.Services;

public class NotificationService(
    IAppDbContext dbContext,
    IHubContext<NotificationHub> hubContext,
    IOneSignalPushService oneSignalPushService,
    ILogger<NotificationService> logger
) : INotificationService
{
    public async Task<long> CreateAndDispatchAsync(
        string userId,
        string title,
        string content,
        NotificationType type = NotificationType.System,
        NotificationRelatedType? relatedType = null,
        long? relatedId = null,
        string? relatedSessionId = null,
        CancellationToken ct = default)
    {
        var entity = new UserNotification
        {
            UserId = userId,
            Title = title.Trim(),
            Content = content.Trim(),
            Type = type,
            RelatedType = relatedType,
            RelatedId = relatedId,
            RelatedSessionId = string.IsNullOrWhiteSpace(relatedSessionId) ? null : relatedSessionId.Trim(),
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.UserNotifications.Add(entity);
        await dbContext.SaveChangesAsync(ct);

        var payload = new RealtimeNotificationDto
        {
            Id = entity.Id,
            UserId = entity.UserId,
            Title = entity.Title,
            Content = entity.Content,
            Type = entity.Type,
            RelatedType = entity.RelatedType,
            RelatedId = entity.RelatedId,
            RelatedSessionId = entity.RelatedSessionId,
            IsRead = entity.IsRead,
            CreatedAt = entity.CreatedAt
        };

        await hubContext.Clients.User(userId)
            .SendAsync("notification.received", payload, ct);

        var activeSubscriptionIds = await dbContext.UserPushTokens
            .Where(x => x.UserId == userId && x.IsActive)
            .Select(x => x.DeviceToken)
            .ToListAsync(ct);

        if (activeSubscriptionIds.Count > 0)
        {
            try
            {
                await oneSignalPushService.SendToSubscriptionIdsAsync(
                    activeSubscriptionIds,
                    entity.Title,
                    entity.Content,
                    new Dictionary<string, string>
                    {
                        ["notificationId"] = entity.Id.ToString(),
                        ["type"] = entity.Type.ToString()
                    },
                    ct);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex,
                    "[NotificationService] OneSignal push failed for notification {NotificationId}, user {UserId}.",
                    entity.Id,
                    userId);
            }
        }

        logger.LogInformation(
            "[NotificationService] Notification {NotificationId} dispatched to user {UserId}.",
            entity.Id,
            userId);

        return entity.Id;
    }
}
