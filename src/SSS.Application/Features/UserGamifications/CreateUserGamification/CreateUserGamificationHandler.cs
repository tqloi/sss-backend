using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserGamifications.Common;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Features.UserGamifications.CreateUserGamification;

public sealed class CreateUserGamificationHandler(
    IAppDbContext dbContext
) : IRequestHandler<CreateUserGamificationCommand, UserGamificationDto>
{
    public async Task<UserGamificationDto> Handle(CreateUserGamificationCommand request, CancellationToken ct)
    {
        // Check if already exists (unique constraint: UserId)
        var exists = await dbContext.UserGamifications
            .AnyAsync(g => g.UserId == request.UserId, ct);

        if (exists)
            throw new InvalidOperationException("User already has a gamification record.");

        var gamification = new UserGamification
        {
            UserId = request.UserId,
            CurrentStreak = request.CurrentStreak,
            LongestStreak = request.LongestStreak,
            LastActiveDate = request.LastActiveDate,
            TotalExp = request.TotalExp,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.UserGamifications.Add(gamification);
        await dbContext.SaveChangesAsync(ct);

        return new UserGamificationDto
        {
            Id = gamification.Id,
            UserId = gamification.UserId,
            CurrentStreak = gamification.CurrentStreak,
            LongestStreak = gamification.LongestStreak,
            LastActiveDate = gamification.LastActiveDate,
            TotalExp = gamification.TotalExp,
            UpdatedAt = gamification.UpdatedAt
        };
    }
}
