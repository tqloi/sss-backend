using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.GetRecentSessions
{
    public class GetRecentSessionsHandler(IAppDbContext context)
        : IRequestHandler<GetRecentSessionsQuery, GetRecentSessionsResult>
    {
        public async Task<GetRecentSessionsResult> Handle(GetRecentSessionsQuery req, CancellationToken ct)
        {
            var sessions = await context.StudySessions
                .AsNoTracking()

                .Where(s => s.UserId == req.UserId && s.Status == SessionStatus.Completed)
                .OrderByDescending(s => s.StartAt)
                .Take(req.Limit)
                .Select(s => new RecentSessionDto
                {
                    Id = s.Id,
                    DurationMinutes = (s.ActualDurationSeconds ?? 0) / 60,
                    TasksCompleted = s.TasksCompletedCount ?? 0,
                    TotalTasks = s.TotalTasks ?? 0,
                    Date = s.StartAt.ToString("yyyy-MM-dd"),
                    Rating = s.SelfRating,
                    NodeTitle = s.SessionTasks.FirstOrDefault() != null ? s.SessionTasks.FirstOrDefault()!.TaskItem.StudyPlanModule.RoadmapNode.Title : null
                })
                .ToListAsync(ct);

            return new GetRecentSessionsResult
            {
                Success = true,
                Data = sessions
            };
        }
    }
}
