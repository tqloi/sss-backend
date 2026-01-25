using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserGamifications.Common;

namespace SSS.Application.Features.UserGamifications.GetUserGamification;

public sealed class GetUserGamificationHandler(
    IAppDbContext dbContext
) : IRequestHandler<GetUserGamificationQuery, UserGamificationDto?>
{
    public async Task<UserGamificationDto?> Handle(GetUserGamificationQuery request, CancellationToken ct)
    {
        var gamification = await dbContext.UserGamifications
            .Where(g => g.UserId == request.UserId)
            .Select(g => new UserGamificationDto
            {
                Id = g.Id,
                UserId = g.UserId,
                CurrentStreak = g.CurrentStreak,
                LongestStreak = g.LongestStreak,
                LastActiveDate = g.LastActiveDate,
                TotalExp = g.TotalExp,
                UpdatedAt = g.UpdatedAt
            })
            .FirstOrDefaultAsync(ct);

        return gamification;
    }
}
