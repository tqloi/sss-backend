using FastEndpoints;
using SSS.Application.Abstractions.Services;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.SendTestNotification;

public sealed class SendTestNotificationEndpoint(
    INotificationService notificationService
) : Endpoint<SendTestNotificationRequest, SendTestNotificationResponse>
{
    public override void Configure()
    {
        Post("/api/notifications/test/send-me");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Send a realtime test notification to current user";
            s.Description = "Creates UserNotification in DB and emits SignalR event to authenticated user.";
        });
    }

    public override async Task HandleAsync(SendTestNotificationRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var notificationId = await notificationService.CreateAndDispatchAsync(
            userId,
            req.Title,
            req.Content,
            req.Type,
            ct: ct);

        await SendOkAsync(new SendTestNotificationResponse
        {
            NotificationId = notificationId,
            Message = "Notification created and sent realtime."
        }, ct);
    }
}
