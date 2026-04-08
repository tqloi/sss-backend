using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Notification.MarkAllAsRead;

public sealed class MarkAllNotificationsAsReadHandler(
    IAppDbContext dbContext
) : IRequestHandler<MarkAllNotificationsAsReadCommand, MarkAllNotificationsAsReadResult>
{
    public async Task<MarkAllNotificationsAsReadResult> Handle(MarkAllNotificationsAsReadCommand request, CancellationToken ct)
    {
        var unreadItems = await dbContext.UserNotifications
            .Where(x => x.UserId == request.UserId && !x.IsRead)
            .ToListAsync(ct);

        if (unreadItems.Count == 0)
        {
            return new MarkAllNotificationsAsReadResult { UpdatedCount = 0 };
        }

        var now = DateTime.UtcNow;
        foreach (var item in unreadItems)
        {
            item.IsRead = true;
            item.ReadAt = now;
        }

        await dbContext.SaveChangesAsync(ct);

        return new MarkAllNotificationsAsReadResult
        {
            UpdatedCount = unreadItems.Count
        };
    }
}
