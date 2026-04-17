using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.GetSessionHistory
{
    public class GetSessionHistoryHandler(IAppDbContext context)
        : IRequestHandler<GetSessionHistoryQuery, GetSessionHistoryResult>
    {
        public async Task<GetSessionHistoryResult> Handle(GetSessionHistoryQuery req, CancellationToken ct)
        {
            var query = context.StudySessions
                .AsNoTracking()

                .Where(s => s.UserId == req.UserId);

            if (req.PlanId.HasValue)
            {
                query = query.Where(s => s.StudyPlanId == req.PlanId.Value);
            }

            // Filter by status
            if (!string.IsNullOrEmpty(req.Status) && Enum.TryParse<SessionStatus>(req.Status, true, out var status))
                query = query.Where(s => s.Status == status);

            // Filter by date range
            if (!string.IsNullOrEmpty(req.StartDate) && DateTime.TryParse(req.StartDate, out var startDate))
                query = query.Where(s => s.StartAt >= startDate);

            if (!string.IsNullOrEmpty(req.EndDate) && DateTime.TryParse(req.EndDate, out var endDate))
                query = query.Where(s => s.StartAt <= endDate.AddDays(1));

            // Sorting
            query = req.SortOrder?.ToLower() == "asc"
                ? query.OrderBy(s => s.StartAt)
                : query.OrderByDescending(s => s.StartAt);

            var totalCount = await query.CountAsync(ct);
            var totalPages = (int)Math.Ceiling(totalCount / (double)req.PageSize);

            var items = await query
                .Skip((req.PageNumber - 1) * req.PageSize)
                .Take(req.PageSize)
                .Select(s => new SessionHistoryItemDto
                {
                    Id = s.Id,
                    Date = s.StartAt.ToString("yyyy-MM-dd"),
                    NodeTitle = s.SessionTasks.FirstOrDefault() != null
                        ? s.SessionTasks.FirstOrDefault()!.TaskItem.StudyPlanModule.RoadmapNode.Title
                        : null,
                    PlanTitle = s.SessionTasks.FirstOrDefault() != null
                        ? s.SessionTasks.FirstOrDefault()!.TaskItem.StudyPlanModule.RoadmapNode.Roadmap.Title
                        : null,
                    DurationSeconds = s.ActualDurationSeconds ?? 0,
                    TasksCompleted = s.TasksCompletedCount ?? 0,
                    TotalTasks = s.TotalTasks ?? 0,
                    XpEarned = s.XpEarned,
                    Rating = s.SelfRating,
                    Status = s.Status.ToString()
                })
                .ToListAsync(ct);

            return new GetSessionHistoryResult
            {
                Success = true,
                Data = new SessionHistoryData
                {
                    Items = items,
                    PageNumber = req.PageNumber,
                    PageSize = req.PageSize,
                    TotalPages = totalPages,
                    TotalCount = totalCount,
                    HasPreviousPage = req.PageNumber > 1,
                    HasNextPage = req.PageNumber < totalPages
                }
            };
        }
    }
}
