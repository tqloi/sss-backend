using FastEndpoints;
using MediatR;
using SSS.Application.Features.Notification.RegisterPushToken;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.RegisterPushToken;

public sealed class RegisterPushTokenEndpoint(
    ISender sender
) : Endpoint<RegisterPushTokenRequest, RegisterPushTokenResponse>
{
    public override void Configure()
    {
        Post("/api/notifications/push-tokens/register");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Register or update push token";
            s.Description = "Upserts a push token for the authenticated user and marks it active.";
        });
    }

    public override async Task HandleAsync(RegisterPushTokenRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var result = await sender.Send(new RegisterPushTokenCommand
        {
            UserId = userId,
            DeviceToken = req.DeviceToken,
            DeviceType = req.DeviceType
        }, ct);

        var response = new RegisterPushTokenResponse
        {
            Id = result.Id,
            UserId = result.UserId,
            DeviceToken = result.DeviceToken,
            DeviceType = result.DeviceType,
            IsActive = result.IsActive,
            LastUpdated = result.LastUpdated
        };

        await SendOkAsync(response, ct);
    }
}
