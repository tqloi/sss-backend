using FastEndpoints;
using MediatR;
using SSS.Application.Features.Notification.MarkAsRead;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadEndpoint(
    ISender sender
) : Endpoint<MarkNotificationAsReadRequest, MarkNotificationAsReadResponse>
{
    public override void Configure()
    {
        Post("/api/notifications/{id}/read");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Mark notification as read";
            s.Description = "Marks a specific notification as read for the authenticated user.";
        });
    }

    public override async Task HandleAsync(MarkNotificationAsReadRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await sender.Send(new MarkNotificationAsReadCommand
        {
            UserId = userId,
            NotificationId = req.Id
        }, ct);

        if (!result.Found)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(new MarkNotificationAsReadResponse
        {
            Id = result.Id,
            IsRead = result.IsRead,
            ReadAt = result.ReadAt
        }, ct);
    }
}
