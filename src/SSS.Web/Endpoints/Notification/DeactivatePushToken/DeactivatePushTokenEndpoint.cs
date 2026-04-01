using FastEndpoints;
using MediatR;
using SSS.Application.Features.Notification.DeactivatePushToken;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.DeactivatePushToken;

public sealed class DeactivatePushTokenEndpoint(
    ISender sender
) : Endpoint<DeactivatePushTokenRequest, DeactivatePushTokenResponse>
{
    public override void Configure()
    {
        Post("/api/notifications/push-tokens/deactivate");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Deactivate a push token";
            s.Description = "Marks a push token as inactive for the authenticated user.";
        });
    }

    public override async Task HandleAsync(DeactivatePushTokenRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await sender.Send(new DeactivatePushTokenCommand
        {
            UserId = userId,
            DeviceToken = req.DeviceToken
        }, ct);

        var response = new DeactivatePushTokenResponse
        {
            Deactivated = result.Deactivated,
            UserId = result.UserId,
            DeviceToken = result.DeviceToken,
            LastUpdated = result.LastUpdated
        };

        await SendOkAsync(response, ct);
    }
}
