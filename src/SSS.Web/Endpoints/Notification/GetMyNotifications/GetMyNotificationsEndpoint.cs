using FastEndpoints;
using MediatR;
using SSS.Application.Features.Notification.GetMyNotifications;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.GetMyNotifications;

public sealed class GetMyNotificationsEndpoint(
    ISender sender
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

        var result = await sender.Send(new GetMyNotificationsQuery
        {
            UserId = userId,
            Page = req.Page,
            PageSize = req.PageSize
        }, ct);

        await SendOkAsync(new GetMyNotificationsResponse
        {
            Page = result.Page,
            PageSize = result.PageSize,
            Total = result.Total,
            UnreadCount = result.UnreadCount,
            Items = result.Items.Select(x => new GetMyNotificationsResponse.NotificationItem
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
            }).ToList()
        }, ct);
    }
}
