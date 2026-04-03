using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Features.UserGamifications.RecordDailyLogin
{
    public class RecordDailyLoginHandler(IAppDbContext context, IUserGamificationRealtimeService realtimeService)
        : IRequestHandler<RecordDailyLoginCommand, RecordDailyLoginResult>
    {
        public async Task<RecordDailyLoginResult> Handle(RecordDailyLoginCommand req, CancellationToken ct)
        {
            var gamification = await context.UserGamifications
                .FirstOrDefaultAsync(g => g.UserId == req.UserId, ct);

            var today = DateTime.UtcNow.Date;
            bool streakUpdatedToday = false;
            string message = "Streak tracking already updated for today.";

            if (gamification == null)
            {
                // First time login
                gamification = new UserGamification
                {
                    UserId = req.UserId,
                    TotalExp = 0,
                    CurrentStreak = 1,
                    LongestStreak = 1,
                    LastActiveDate = today,
                    UpdatedAt = DateTime.UtcNow
                };
                context.UserGamifications.Add(gamification);
                streakUpdatedToday = true;
                message = "First time login recorded! Streak started.";
            }
            else
            {
                var lastActiveDate = gamification.LastActiveDate?.Date;

                if (lastActiveDate != today)
                {
                    if (lastActiveDate == today.AddDays(-1))
                    {
                        gamification.CurrentStreak = (gamification.CurrentStreak ?? 0) + 1;
                        if (gamification.CurrentStreak > (gamification.LongestStreak ?? 0))
                        {
                            gamification.LongestStreak = gamification.CurrentStreak;
                        }
                        message = $"Streak increased! Current streak: {gamification.CurrentStreak} days.";
                    }
                    else
                    {
                        gamification.CurrentStreak = 1;
                        message = "Streak lost. Starting over!";
                    }
                    
                    gamification.LastActiveDate = today;
                    gamification.UpdatedAt = DateTime.UtcNow;
                    streakUpdatedToday = true;
                }
            }

            await context.SaveChangesAsync(ct);

            var commonDto = new Common.UserGamificationDto
            {
                Id = gamification.Id,
                UserId = gamification.UserId,
                CurrentStreak = gamification.CurrentStreak,
                LongestStreak = gamification.LongestStreak,
                LastActiveDate = gamification.LastActiveDate,
                TotalExp = gamification.TotalExp,
                UpdatedAt = gamification.UpdatedAt
            };

            await realtimeService.NotifyGamificationUpdatedAsync(req.UserId, commonDto, ct);

            return new RecordDailyLoginResult
            {
                Success = true,
                Message = message,
                Data = new UserGamificationDto
                {
                    CurrentStreak = gamification.CurrentStreak ?? 0,
                    LongestStreak = gamification.LongestStreak ?? 0,
                    TotalExp = gamification.TotalExp ?? 0,
                    StreakUpdatedToday = streakUpdatedToday
                }
            };
        }
    }
}
