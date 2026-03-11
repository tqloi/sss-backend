using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.GetSessionStatistics
{
    public class GetSessionStatisticsHandler(IAppDbContext context)
        : IRequestHandler<GetSessionStatisticsQuery, GetSessionStatisticsResult>
    {
        public async Task<GetSessionStatisticsResult> Handle(GetSessionStatisticsQuery req, CancellationToken ct)
        {
            var query = context.StudySessions
                .AsNoTracking()
                .Where(s => s.UserId == req.UserId);

            // Period filter
            var now = DateTime.UtcNow;
            DateTime? periodStart = req.Period?.ToLower() switch
            {
                "week" => now.AddDays(-7),
                "month" => now.AddMonths(-1),
                _ => null
            };

            if (periodStart.HasValue)
                query = query.Where(s => s.StartAt >= periodStart.Value);

            var sessions = await query.ToListAsync(ct);

            var completedSessions = sessions.Where(s => s.Status == SessionStatus.Completed).ToList();
            var totalMinutes = completedSessions.Sum(s => (s.ActualDurationSeconds ?? 0)) / 60;
            var avgLength = completedSessions.Count > 0 ? totalMinutes / completedSessions.Count : 0;
            var completionRate = sessions.Count > 0 ? (double)completedSessions.Count / sessions.Count : 0;

            // This week stats
            var weekStart = now.AddDays(-7);
            var thisWeekSessions = sessions.Where(s => s.StartAt >= weekStart).ToList();
            var minutesThisWeek = thisWeekSessions
                .Where(s => s.Status == SessionStatus.Completed)
                .Sum(s => (s.ActualDurationSeconds ?? 0)) / 60;

            // Streak calculation (consecutive days with completed sessions)
            var currentStreak = CalculateStreak(completedSessions, now);

            // Average rating
            var ratedSessions = completedSessions.Where(s => s.SelfRating.HasValue).ToList();
            var avgRating = ratedSessions.Count > 0
                ? ratedSessions.Average(s => s.SelfRating!.Value)
                : 0;

            // Total XP
            var totalXp = completedSessions.Sum(s => ((s.ActiveSeconds ?? 0) / 60) * 10);

            return new GetSessionStatisticsResult
            {
                Success = true,
                Data = new SessionStatisticsDto
                {
                    TotalSessions = sessions.Count,
                    TotalMinutes = totalMinutes,
                    AverageSessionLength = avgLength,
                    CompletionRate = Math.Round(completionRate, 2),
                    CurrentStreak = currentStreak,
                    LongestStreak = currentStreak, // simplified
                    SessionsThisWeek = thisWeekSessions.Count,
                    MinutesThisWeek = minutesThisWeek,
                    TotalXpEarned = totalXp,
                    AverageRating = Math.Round(avgRating, 1)
                }
            };
        }

        private static int CalculateStreak(List<Domain.Entities.Tracking.StudySession> sessions, DateTime now)
        {
            if (sessions.Count == 0) return 0;

            var dates = sessions
                .Select(s => s.StartAt.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            var streak = 0;
            var checkDate = now.Date;

            foreach (var date in dates)
            {
                if (date == checkDate || date == checkDate.AddDays(-1))
                {
                    streak++;
                    checkDate = date;
                }
                else break;
            }

            return streak;
        }
    }
}
