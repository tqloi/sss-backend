using FastEndpoints;
using MediatR;
using SSS.Application.Features.UserGamifications.GetUserGamification;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserGamifications.Get;

public sealed class GetUserGamificationRequest
{
    public long Id { get; set; }
}

public sealed class GetUserGamificationEndpoint(
    ISender sender,
    AutoMapper.IMapper mapper
) : Endpoint<GetUserGamificationRequest, GetUserGamificationResponse>
{
    public override void Configure()
    {
        Get("/api/user-gamifications/{id}");
        Description(d => d.WithTags("UserGamifications"));
        Summary(s =>
        {
            s.Summary = "Get user gamification by ID";
            s.Description = "Returns the gamification data by ID for the authenticated user.";
        });
    }

    public override async Task HandleAsync(GetUserGamificationRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var query = new GetUserGamificationQuery { UserId = userId };
        var result = await sender.Send(query, ct);

        if (result is null || result.Id != req.Id)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        var response = mapper.Map<GetUserGamificationResponse>(result);
        await SendOkAsync(response, ct);
    }
}
