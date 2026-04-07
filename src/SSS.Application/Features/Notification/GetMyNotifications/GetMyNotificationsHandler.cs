using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Notification.GetMyNotifications;

public sealed class GetMyNotificationsHandler(
    IAppDbContext dbContext
) : IRequestHandler<GetMyNotificationsQuery, GetMyNotificationsResult>
{
    public async Task<GetMyNotificationsResult> Handle(GetMyNotificationsQuery request, CancellationToken ct)
    {
        var total = await dbContext.UserNotifications
            .Where(x => x.UserId == request.UserId)
            .CountAsync(ct);

        var unreadCount = await dbContext.UserNotifications
            .Where(x => x.UserId == request.UserId && !x.IsRead)
            .CountAsync(ct);

        var items = await dbContext.UserNotifications
            .Where(x => x.UserId == request.UserId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new GetMyNotificationsResult.NotificationItem
            {
                Id = x.Id,
                Title = x.Title,
                Content = x.Content,
                Type = x.Type,
                RelatedType = x.RelatedType,
                RelatedId = x.RelatedId,
                RelatedSessionId = x.RelatedSessionId,
                IsRead = x.IsRead,
                ReadAt = x.ReadAt,
                CreatedAt = x.CreatedAt
            })
            .ToListAsync(ct);

        return new GetMyNotificationsResult
        {
            Page = request.Page,
            PageSize = request.PageSize,
            Total = total,
            UnreadCount = unreadCount,
            Items = items
        };
    }
}
