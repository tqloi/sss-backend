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
            var baseQuery = context.StudySessions
                .AsNoTracking()
                .Where(s => s.UserId == req.UserId);

            if (req.PlanId.HasValue)
            {
                baseQuery = baseQuery.Where(s => s.StudyPlanId == req.PlanId.Value);
            }

            // Period filter
            var now = DateTime.UtcNow;
            DateTime? periodStart = req.Period?.ToLower() switch
            {
                "week" => now.AddDays(-7),
                "month" => now.AddMonths(-1),
                _ => null
            };

            if (periodStart.HasValue)
                baseQuery = baseQuery.Where(s => s.StartAt >= periodStart.Value);

            // Project only needed fields — avoid loading full entity graph
            var sessions = await baseQuery
                .Select(s => new
                {
                    s.Status,
                    s.StartAt,
                    s.ActualDurationSeconds,
                    s.SelfRating,
                    s.TasksCompletedCount,
                    s.XpEarned
                })
                .ToListAsync(ct);

            var completedSessions = sessions.Where(s => s.Status == SessionStatus.Completed).ToList();
            var totalSeconds = completedSessions.Sum(s => (s.ActualDurationSeconds ?? 0));
            var avgLengthSeconds = completedSessions.Count > 0 ? (int)totalSeconds / completedSessions.Count : 0;
            var completionRate = sessions.Count > 0 ? (double)completedSessions.Count / sessions.Count : 0;

            // This week stats
            var weekStart = now.AddDays(-7);
            var thisWeekSessions = sessions.Where(s => s.StartAt >= weekStart).ToList();
            var secondsThisWeek = thisWeekSessions
                .Where(s => s.Status == SessionStatus.Completed)
                .Sum(s => (s.ActualDurationSeconds ?? 0));

            // Streak calculations
            var completedDates = completedSessions
                .Select(s => s.StartAt.Date)
                .Distinct()
                .OrderByDescending(d => d)
                .ToList();

            var currentStreak = CalculateCurrentStreak(completedDates, now);
            var longestStreak = CalculateLongestStreak(completedDates);

            // Average rating
            var ratedSessions = completedSessions.Where(s => s.SelfRating.HasValue).ToList();
            var avgRating = ratedSessions.Count > 0
                ? ratedSessions.Average(s => s.SelfRating!.Value)
                : 0;

            // Total XP — read persisted value, not recompute
            var totalXp = completedSessions.Sum(s => s.XpEarned);

            return new GetSessionStatisticsResult
            {
                Success = true,
                Data = new SessionStatisticsDto
                {
                    TotalSessions = sessions.Count,
                    TotalSeconds = (int)totalSeconds,
                    AverageSessionLengthSeconds = avgLengthSeconds,
                    CompletionRate = Math.Round(completionRate, 2),
                    CurrentStreak = currentStreak,
                    LongestStreak = longestStreak,
                    SessionsThisWeek = thisWeekSessions.Count,
                    SecondsThisWeek = (int)secondsThisWeek,
                    TotalXpEarned = totalXp,
                    AverageRating = Math.Round(avgRating, 1)
                }
            };
        }

        private static int CalculateCurrentStreak(List<DateTime> orderedDates, DateTime now)
        {
            if (orderedDates.Count == 0) return 0;

            var streak = 0;
            var checkDate = now.Date;

            foreach (var date in orderedDates)
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

        private static int CalculateLongestStreak(List<DateTime> orderedDates)
        {
            if (orderedDates.Count == 0) return 0;

            // orderedDates is descending, reverse for ascending traversal
            var ascending = orderedDates.AsEnumerable().Reverse().ToList();

            var longest = 1;
            var current = 1;

            for (var i = 1; i < ascending.Count; i++)
            {
                if ((ascending[i] - ascending[i - 1]).Days == 1)
                {
                    current++;
                    if (current > longest) longest = current;
                }
                else
                {
                    current = 1;
                }
            }

            return longest;
        }
    }
}
