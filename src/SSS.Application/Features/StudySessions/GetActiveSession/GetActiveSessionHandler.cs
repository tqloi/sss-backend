using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.GetActiveSession
{
    public class GetActiveSessionHandler(IAppDbContext context)
        : IRequestHandler<GetActiveSessionQuery, GetActiveSessionResult>
    {
        public async Task<GetActiveSessionResult> Handle(GetActiveSessionQuery req, CancellationToken ct)
        {
            var query = context.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                        .ThenInclude(t => t.StudyPlanModule)
                            .ThenInclude(m => m.RoadmapNode)
                                .ThenInclude(n => n!.Roadmap)
                                    .ThenInclude(r => r!.StudyPlans)
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
            {
                return new GetActiveSessionResult { Success = true, Data = null };
            }

            var totalElapsed = (int)(DateTime.UtcNow - session.StartAt).TotalSeconds;
            var elapsedSeconds = totalElapsed - session.PauseSeconds;

            if (session.Status == SessionStatus.Paused && session.PausedAt.HasValue)
            {
                var currentPauseDuration = (int)(DateTime.UtcNow - session.PausedAt.Value).TotalSeconds;
                elapsedSeconds -= currentPauseDuration;
            }

            var firstTask = session.SessionTasks.FirstOrDefault()?.TaskItem;
            var node = firstTask?.StudyPlanModule?.RoadmapNode;
            var plan = node?.Roadmap?.StudyPlans?.FirstOrDefault();

            return new GetActiveSessionResult
            {
                Success = true,
                Data = new ActiveSessionDto
                {
                    SessionId = session.Id,
                    Status = session.Status.ToString(),
                    StartAt = session.StartAt,
                    ElapsedSeconds = elapsedSeconds,
                    PlanId = plan?.Id,
                    NodeId = node?.Id,
                    NodeTitle = node?.Title,
                    PlanTitle = node?.Roadmap?.Title,
                    Tasks = session.SessionTasks.Select(st => new SessionTaskDto
                    {
                        Id = st.TaskId,
                        Title = st.TaskItem?.Title ?? "",
                        Description = st.TaskItem?.Description,
                        Order = 0,
                        EstimatedMinutes = st.TaskItem?.EstimatedDurationSeconds / 60,
                        IsCompleted = st.TaskItem != null && st.TaskItem.Status == Domain.Enums.TaskStatus.Completed
                    }).ToList()
                }
            };
        }
    }
}
