using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserGamifications.CreateUserGamification;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserGamifications.Create;

public sealed class CreateUserGamificationEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<CreateUserGamificationRequest, CreateUserGamificationResponse>
{
    public override void Configure()
    {
        Post("/api/user-gamifications");
        Description(d => d.WithTags("UserGamifications"));
        Summary(s =>
        {
            s.Summary = "Create user gamification";
            s.Description = "Creates a new gamification record for the authenticated user.";
        });
    }

    public override async Task HandleAsync(CreateUserGamificationRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        try
        {
            var command = mapper.Map<CreateUserGamificationCommand>(req);
            command.UserId = userId;

            var result = await sender.Send(command, ct);
            var response = mapper.Map<CreateUserGamificationResponse>(result);
            await SendCreatedAtAsync<Get.GetUserGamificationEndpoint>(
                routeValues: null,
                response,
                cancellation: ct
            );
        }
        catch (InvalidOperationException ex)
        {
            AddError(ex.Message);
            await SendErrorsAsync(statusCode: 400, cancellation: ct);
        }
    }
}
