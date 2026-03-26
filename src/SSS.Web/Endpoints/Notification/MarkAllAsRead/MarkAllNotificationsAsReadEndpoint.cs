using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.MarkAllAsRead;

public sealed class MarkAllNotificationsAsReadEndpoint(
    IAppDbContext dbContext
) : EndpointWithoutRequest<MarkAllNotificationsAsReadResponse>
{
    public override void Configure()
    {
        Post("/api/notifications/read-all");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Mark all notifications as read";
            s.Description = "Marks all unread notifications as read for the authenticated user.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var unreadItems = await dbContext.UserNotifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .ToListAsync(ct);

        if (unreadItems.Count == 0)
        {
            await SendOkAsync(new MarkAllNotificationsAsReadResponse { UpdatedCount = 0 }, ct);
            return;
        }

        var now = DateTime.UtcNow;
        foreach (var item in unreadItems)
        {
            item.IsRead = true;
            item.ReadAt = now;
        }

        await dbContext.SaveChangesAsync(ct);

        await SendOkAsync(new MarkAllNotificationsAsReadResponse
        {
            UpdatedCount = unreadItems.Count
        }, ct);
    }
}
