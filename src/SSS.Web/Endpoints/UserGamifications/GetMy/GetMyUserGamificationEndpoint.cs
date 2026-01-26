using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserGamifications.GetMy;

public sealed class GetMyUserGamificationEndpoint(
    IAppDbContext dbContext
) : EndpointWithoutRequest<GetMyUserGamificationResponse>
{
    public override void Configure()
    {
        Get("/api/user-gamifications/me");
        Description(d => d.WithTags("UserGamifications"));
        Summary(s =>
        {
            s.Summary = "Get my gamification";
            s.Description = "Returns the gamification data (streaks, XP) for the authenticated user.";
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

        var gamification = await dbContext.UserGamifications
            .Where(g => g.UserId == userId)
            .Select(g => new GetMyUserGamificationResponse
            {
                Id = g.Id,
                CurrentStreak = g.CurrentStreak,
                LongestStreak = g.LongestStreak,
                LastActiveDate = g.LastActiveDate,
                TotalExp = g.TotalExp,
                UpdatedAt = g.UpdatedAt
            })
            .FirstOrDefaultAsync(ct);

        if (gamification is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        await SendOkAsync(gamification, ct);
    }
}
