using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.WebApi.Endpoints.UserGamifications.GetAll;

public sealed class GetAllUserGamificationsEndpoint(
    IAppDbContext dbContext
) : EndpointWithoutRequest<GetAllUserGamificationsResponse>
{
    public override void Configure()
    {
        Get("/api/user-gamifications");
        Roles("Admin");
        Tags("UserGamifications");
        Summary(s =>
        {
            s.Summary = "Get all user gamifications (Admin only)";
            s.Description = "Returns all user gamification records. Requires Admin role.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var gamifications = await dbContext.UserGamifications
            .OrderByDescending(g => g.TotalExp)
            .Select(g => new GetAllUserGamificationsResponse.UserGamificationItem
            {
                Id = g.Id,
                UserId = g.UserId,
                CurrentStreak = g.CurrentStreak,
                LongestStreak = g.LongestStreak,
                LastActiveDate = g.LastActiveDate,
                TotalExp = g.TotalExp,
                UpdatedAt = g.UpdatedAt
            })
            .ToListAsync(ct);

        var response = new GetAllUserGamificationsResponse { Items = gamifications };
        await SendOkAsync(response, ct);
    }
}
