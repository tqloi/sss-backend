using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserGamifications.Common;

namespace SSS.Application.Features.UserGamifications.UpdateUserGamification;

public sealed class UpdateUserGamificationHandler(
    IAppDbContext dbContext
) : IRequestHandler<UpdateUserGamificationCommand, UserGamificationDto>
{
    public async Task<UserGamificationDto> Handle(UpdateUserGamificationCommand request, CancellationToken ct)
    {
        var existing = await dbContext.UserGamifications
            .FirstOrDefaultAsync(g => g.UserId == request.UserId, ct);

        if (existing is null)
            throw new InvalidOperationException("User gamification not found.");

        // Update fields if provided
        if (request.CurrentStreak.HasValue)
            existing.CurrentStreak = request.CurrentStreak;

        if (request.LongestStreak.HasValue)
            existing.LongestStreak = request.LongestStreak;

        if (request.LastActiveDate.HasValue)
            existing.LastActiveDate = request.LastActiveDate;

        if (request.TotalExp.HasValue)
            existing.TotalExp = request.TotalExp;

        existing.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(ct);

        return new UserGamificationDto
        {
            Id = existing.Id,
            UserId = existing.UserId,
            CurrentStreak = existing.CurrentStreak,
            LongestStreak = existing.LongestStreak,
            LastActiveDate = existing.LastActiveDate,
            TotalExp = existing.TotalExp,
            UpdatedAt = existing.UpdatedAt
        };
    }
}
