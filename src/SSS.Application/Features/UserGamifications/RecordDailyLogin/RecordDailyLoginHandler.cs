using System.Data;
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
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            var now = DateTime.UtcNow;

            await using var tx = await context.BeginTransactionAsync(IsolationLevel.Serializable, ct);

            UserGamification? gamification = await context.UserGamifications
                .FromSqlInterpolated($@"
                    SELECT * FROM UserGamifications WITH (UPDLOCK, HOLDLOCK)
                    WHERE UserId = {req.UserId}")
                .SingleOrDefaultAsync(ct);

            bool streakUpdatedToday = false;
            string message = "Streak tracking already updated for today.";

            if (gamification == null)
            {
                gamification = new UserGamification
                {
                    UserId = req.UserId,
                    TotalExp = 0,
                    CurrentStreak = 1,
                    LongestStreak = 1,
                    LastActiveDate = today.ToDateTime(TimeOnly.MinValue),
                    UpdatedAt = now
                };

                context.UserGamifications.Add(gamification);

                try
                {
                    await context.SaveChangesAsync(ct);
                    streakUpdatedToday = true;
                    message = "First time login recorded! Streak started.";
                }
                catch (DbUpdateException ex) when (IsUniqueUserIdViolation(ex))
                {
                    // Request đồng thời đã tạo record trước đó
                    gamification = await context.UserGamifications
                        .FromSqlInterpolated($@"
                            SELECT * FROM UserGamifications WITH (UPDLOCK, HOLDLOCK)
                            WHERE UserId = {req.UserId}")
                        .SingleAsync(ct);
                }
            }

            if (gamification != null)
            {
                var last = gamification.LastActiveDate?.Date;

                if (last != today.ToDateTime(TimeOnly.MinValue).Date)
                {
                    if (last == today.AddDays(-1).ToDateTime(TimeOnly.MinValue).Date)
                    {
                        gamification.CurrentStreak = (gamification.CurrentStreak ?? 0) + 1;
                        gamification.LongestStreak = Math.Max(gamification.LongestStreak ?? 0, gamification.CurrentStreak ?? 0);
                        message = $"Streak increased! Current streak: {gamification.CurrentStreak} days.";
                    }
                    else
                    {
                        gamification.CurrentStreak = 1;
                        gamification.LongestStreak = Math.Max(gamification.LongestStreak ?? 0, 1);
                        message = "Streak lost. Starting over!";
                    }

                    gamification.LastActiveDate = today.ToDateTime(TimeOnly.MinValue);
                    gamification.UpdatedAt = now;
                    streakUpdatedToday = true;

                    await context.SaveChangesAsync(ct);
                }
            }

            await tx.CommitAsync(ct);

            // Chỉ notify khi có thay đổi thật
            if (streakUpdatedToday)
            {
                var dto = new Common.UserGamificationDto
                {
                    Id = gamification!.Id,
                    UserId = gamification.UserId,
                    CurrentStreak = gamification.CurrentStreak,
                    LongestStreak = gamification.LongestStreak,
                    LastActiveDate = gamification.LastActiveDate,
                    TotalExp = gamification.TotalExp,
                    UpdatedAt = gamification.UpdatedAt
                };

                await realtimeService.NotifyGamificationUpdatedAsync(req.UserId, dto, ct);
            }

            return new RecordDailyLoginResult
            {
                Success = true,
                Message = message,
                Data = new UserGamificationDto
                {
                    CurrentStreak = gamification!.CurrentStreak ?? 0,
                    LongestStreak = gamification.LongestStreak ?? 0,
                    TotalExp = gamification.TotalExp ?? 0,
                    StreakUpdatedToday = streakUpdatedToday
                }
            };
        }

        private static bool IsUniqueUserIdViolation(DbUpdateException ex)
        {
            if (ex.InnerException == null) return false;

            var innerType = ex.InnerException.GetType();
            if (innerType.Name == "SqlException")
            {
                var numberProperty = innerType.GetProperty("Number");
                if (numberProperty != null)
                {
                    var number = (int?)numberProperty.GetValue(ex.InnerException);
                    return number == 2601 || number == 2627;
                }
            }
            return false;
        }
    }
}
