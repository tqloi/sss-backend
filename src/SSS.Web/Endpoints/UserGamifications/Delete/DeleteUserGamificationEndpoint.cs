using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserGamifications.DeleteUserGamification;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserGamifications.Delete;

public sealed class DeleteUserGamificationEndpoint(
    ISender sender
) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Delete("/api/user-gamifications/{id}");
        Tags("UserGamifications");
        Summary(s =>
        {
            s.Summary = "Delete user gamification";
            s.Description = "Deletes the gamification record for the authenticated user.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var command = new DeleteUserGamificationCommand { UserId = userId };
        var deleted = await sender.Send(command, ct);

        if (!deleted)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendNoContentAsync(ct);
    }
}
