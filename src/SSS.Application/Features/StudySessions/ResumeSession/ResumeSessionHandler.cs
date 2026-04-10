using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.ResumeSession
{
    public class ResumeSessionHandler(IAppDbContext context, ICacheService cache)
        : IRequestHandler<ResumeSessionCommand, ResumeSessionResult>
    {
        public async Task<ResumeSessionResult> Handle(ResumeSessionCommand req, CancellationToken ct)
        {
            var cacheKey = $"StudySession:Active:{req.UserId}";
            var activeSession = await cache.GetAsync<ActiveSessionCacheDto>(cacheKey);

            if (activeSession == null)
            {
                // Fallback
                var session = await context.StudySessions
                    .Include(s => s.SessionTasks)
                    .FirstOrDefaultAsync(s => s.Id == req.SessionId && s.UserId == req.UserId, ct)
                    ?? throw new NotFoundException($"Session {req.SessionId} not found");

                activeSession = new ActiveSessionCacheDto
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
            }

            if (activeSession.Id != req.SessionId)
                throw new ConflictException("Specified session is not the active one.");

            if (activeSession.Status != SessionStatus.Paused)
                throw new ConflictException("Only Paused sessions can be resumed.");

            // Accumulate pause time
            if (activeSession.PausedAt.HasValue)
            {
                activeSession.PauseSeconds += (int)(DateTime.UtcNow - activeSession.PausedAt.Value).TotalSeconds;
            }

            activeSession.Status = SessionStatus.InProgress;
            activeSession.PausedAt = null;

            await cache.SetAsync(cacheKey, activeSession, TimeSpan.FromHours(12));

            return new ResumeSessionResult
            {
                Success = true,
                Message = "Session resumed",
                Data = new ResumeSessionResponse
                {
                    SessionId = activeSession.Id,
                    Status = activeSession.Status.ToString()
                }
            };
        }
    }
}
