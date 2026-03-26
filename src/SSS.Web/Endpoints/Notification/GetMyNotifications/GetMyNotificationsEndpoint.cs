using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.GetMyNotifications;

public sealed class GetMyNotificationsEndpoint(
    IAppDbContext dbContext
) : Endpoint<GetMyNotificationsRequest, GetMyNotificationsResponse>
{
    public override void Configure()
    {
        Get("/api/notifications/me");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Get my notifications";
            s.Description = "Returns paginated notifications and unread count for the authenticated user.";
        });
    }

    public override async Task HandleAsync(GetMyNotificationsRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var total = await dbContext.UserNotifications
            .Where(x => x.UserId == userId)
            .CountAsync(ct);

        var unreadCount = await dbContext.UserNotifications
            .Where(x => x.UserId == userId && !x.IsRead)
            .CountAsync(ct);

        var items = await dbContext.UserNotifications
            .Where(x => x.UserId == userId)
            .OrderByDescending(x => x.CreatedAt)
            .Skip((req.Page - 1) * req.PageSize)
            .Take(req.PageSize)
            .Select(x => new GetMyNotificationsResponse.NotificationItem
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

        await SendOkAsync(new GetMyNotificationsResponse
        {
            Page = req.Page,
            PageSize = req.PageSize,
            Total = total,
            UnreadCount = unreadCount,
            Items = items
        }, ct);
    }
}
