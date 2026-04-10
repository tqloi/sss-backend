using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Entities.Tracking;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.GetActiveSession
{
    public class GetActiveSessionHandler(IAppDbContext context, ICacheService cache)
        : IRequestHandler<GetActiveSessionQuery, GetActiveSessionResult>
    {
        public async Task<GetActiveSessionResult> Handle(GetActiveSessionQuery req, CancellationToken ct)
        {
            var cacheKey = $"StudySession:Active:{req.UserId}";
            var cachedSession = await cache.GetAsync<ActiveSessionCacheDto>(cacheKey);

            if (cachedSession != null)
            {
                if (req.PlanId.HasValue && cachedSession.StudyPlanId != req.PlanId.Value)
                    return new GetActiveSessionResult { Success = true, Data = null };

                var cElapsedSeconds = (int)(DateTime.UtcNow - cachedSession.StartAt).TotalSeconds - cachedSession.PauseSeconds;
                if (cachedSession.Status == SessionStatus.Paused && cachedSession.PausedAt.HasValue)
                {
                    cElapsedSeconds -= (int)(DateTime.UtcNow - cachedSession.PausedAt.Value).TotalSeconds;
                }

                // Get static display data from DB
                var taskIds = cachedSession.Tasks.Select(t => t.TaskId).ToList();
                var tasksData = await context.TaskItems
                    .AsNoTracking()
                    .Include(t => t.StudyPlanModule)
                        .ThenInclude(m => m.RoadmapNode)
                            .ThenInclude(n => n!.Roadmap)
                    .Where(t => taskIds.Contains(t.Id))
                    .ToListAsync(ct);

                var firstTask = tasksData.FirstOrDefault();
                var node = firstTask?.StudyPlanModule?.RoadmapNode;

                return new GetActiveSessionResult
                {
                    Success = true,
                    Data = new ActiveSessionDto
                    {
                        SessionId = cachedSession.Id,
                        Status = cachedSession.Status.ToString(),
                        StartAt = cachedSession.StartAt,
                        ElapsedSeconds = cElapsedSeconds,
                        PlanId = cachedSession.StudyPlanId,
                        NodeId = node?.Id,
                        NodeTitle = node?.Title,
                        PlanTitle = node?.Roadmap?.Title,
                        Tasks = tasksData.Select(t => new SessionTaskDto
                        {
                            Id = t.Id,
                            Title = t.Title ?? "",
                            Description = t.Description,
                            Order = 0,
                            EstimatedMinutes = t.EstimatedDurationSeconds / 60,
                            IsCompleted = t.Status == Domain.Enums.TaskStatus.Completed
                        }).ToList()
                    }
                };
            }

            // Fallback to DB
            var query = context.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                        .ThenInclude(t => t.StudyPlanModule)
                            .ThenInclude(m => m.RoadmapNode)
                                .ThenInclude(n => n!.Roadmap)
                .Where(s => s.UserId == req.UserId &&
                            (s.Status == SessionStatus.InProgress || s.Status == SessionStatus.Paused));

            if (req.PlanId.HasValue)
            {
                query = query.Where(s => s.StudyPlanId == req.PlanId.Value);
            }

            var session = await query
                .OrderByDescending(s => s.StartAt)
                .FirstOrDefaultAsync(ct);

            if (session == null)
                return new GetActiveSessionResult { Success = true, Data = null };

            var totalElapsed = (int)(DateTime.UtcNow - session.StartAt).TotalSeconds;
            var elapsedSeconds = totalElapsed - session.PauseSeconds;

            if (session.Status == SessionStatus.Paused && session.PausedAt.HasValue)
            {
                var currentPauseDuration = (int)(DateTime.UtcNow - session.PausedAt.Value).TotalSeconds;
                elapsedSeconds -= currentPauseDuration;
            }

            var fTask = session.SessionTasks.FirstOrDefault()?.TaskItem;
            var fNode = fTask?.StudyPlanModule?.RoadmapNode;

            var resultDto = new ActiveSessionDto
            {
                SessionId = session.Id,
                Status = session.Status.ToString(),
                StartAt = session.StartAt,
                ElapsedSeconds = elapsedSeconds,
                PlanId = session.StudyPlanId,
                NodeId = fNode?.Id,
                NodeTitle = fNode?.Title,
                PlanTitle = fNode?.Roadmap?.Title,
                Tasks = session.SessionTasks.Select(st => new SessionTaskDto
                {
                    Id = st.TaskId,
                    Title = st.TaskItem?.Title ?? "",
                    Description = st.TaskItem?.Description,
                    Order = 0,
                    EstimatedMinutes = st.TaskItem?.EstimatedDurationSeconds / 60,
                    IsCompleted = st.TaskItem != null && st.TaskItem.Status == Domain.Enums.TaskStatus.Completed
                }).ToList()
            };

            // Cache it for next time
            var fallbackCacheDto = new ActiveSessionCacheDto
            {
                Id = session.Id,
                UserId = session.UserId,
                Status = session.Status,
                StartAt = session.StartAt,
                PausedAt = session.PausedAt,
                PauseCount = session.PauseCount,
                PauseSeconds = session.PauseSeconds,
                StudyPlanId = session.StudyPlanId,
                StudyPlanModuleId = session.StudyPlanModuleId,
                PlannedDurationSeconds = session.PlannedDurationSeconds,
                Timezone = session.Timezone,
                Tasks = session.SessionTasks.Select(t => new ActiveSessionTaskCacheDto
                {
                    Id = t.Id,
                    TaskId = t.TaskId,
                    Status = t.Status,
                    StartTimeUtc = t.StartTimeUtc,
                    EndTimeUtc = t.EndTimeUtc
                }).ToList()
            };
            await cache.SetAsync(cacheKey, fallbackCacheDto, TimeSpan.FromHours(12));

            return new GetActiveSessionResult
            {
                Success = true,
                Data = resultDto
            };
        }
    }
}
