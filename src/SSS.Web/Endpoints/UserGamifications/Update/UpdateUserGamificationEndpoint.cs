using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserGamifications.UpdateUserGamification;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserGamifications.Update;

public sealed class UpdateUserGamificationEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<UpdateUserGamificationRequest, UpdateUserGamificationResponse>
{
    public override void Configure()
    {
        Put("/api/user-gamifications/{id}");
        Tags("UserGamifications");
        Summary(s =>
        {
            s.Summary = "Update user gamification";
            s.Description = "Updates the gamification record for the authenticated user.";
        });
    }

    public override async Task HandleAsync(UpdateUserGamificationRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var command = mapper.Map<UpdateUserGamificationCommand>(req);
            command.UserId = userId;

            var result = await sender.Send(command, ct);
            var response = mapper.Map<UpdateUserGamificationResponse>(result);
            await SendOkAsync(response, ct);
        }
        catch (InvalidOperationException ex)
        {
            if (ex.Message.Contains("not found"))
            {
                await SendNotFoundAsync(ct);
            }
            else
            {
                AddError(ex.Message);
                await SendErrorsAsync(statusCode: 400, cancellation: ct);
            }
        }
    }
}
