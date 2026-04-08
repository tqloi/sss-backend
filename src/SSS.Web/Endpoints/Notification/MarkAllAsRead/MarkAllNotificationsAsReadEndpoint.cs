using FastEndpoints;
using MediatR;
using SSS.Application.Features.Notification.MarkAllAsRead;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.MarkAllAsRead;

public sealed class MarkAllNotificationsAsReadEndpoint(
    ISender sender
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

        var result = await sender.Send(new MarkAllNotificationsAsReadCommand
        {
            UserId = userId
        }, ct);

        await SendOkAsync(new MarkAllNotificationsAsReadResponse
        {
            UpdatedCount = result.UpdatedCount
        }, ct);
    }
}
