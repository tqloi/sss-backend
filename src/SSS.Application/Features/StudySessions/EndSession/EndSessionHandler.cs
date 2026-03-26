using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.EndSession
{
    public class EndSessionHandler(IAppDbContext context, ICacheService cacheService)
        : IRequestHandler<EndSessionCommand, EndSessionResult>
    {
        public async Task<EndSessionResult> Handle(EndSessionCommand req, CancellationToken ct)
        {
            var session = await context.StudySessions
                .Include(s => s.SessionTasks)
                .FirstOrDefaultAsync(s => s.Id == req.SessionId && s.UserId == req.UserId, ct)
                ?? throw new NotFoundException($"Session {req.SessionId} not found");

            if (session.Status == SessionStatus.Completed || session.Status == SessionStatus.Cancelled)
                throw new ConflictException("Session has already ended.");

            // Update session
            session.Status = SessionStatus.Completed;
            session.EndAt = DateTime.UtcNow;
            session.SelfRating = req.SelfRating;

            // Parse ended reason
            if (Enum.TryParse<SessionEndedReason>(req.EndedReason, true, out var reason))
                session.EndedReason = reason;
            else
                session.EndedReason = SessionEndedReason.Completed;

            // Calculate durations
            var totalSeconds = (int)(session.EndAt.Value - session.StartAt).TotalSeconds;
            session.ActualDurationSeconds = req.ActualDurationSeconds ?? totalSeconds;

            // If session was paused when ended, accumulate remaining pause time
            if (session.PausedAt.HasValue)
            {
                session.PauseSeconds += (int)(DateTime.UtcNow - session.PausedAt.Value).TotalSeconds;
                session.PausedAt = null;
            }

            // Mark tasks as completed
            var tasksCompletedCount = 0;
            var totalTasks = session.TotalTasks ?? 0;
            if (req.Tasks is { Count: > 0 })
            {
                var taskIds = req.Tasks.Select(t => t.TaskId).ToList();
                var tasksToUpdate = await context.TaskItems
                    .Where(t => taskIds.Contains(t.Id))
                    .ToListAsync(ct);

                foreach (var taskInfo in req.Tasks)
                {
                    var task = tasksToUpdate.FirstOrDefault(t => t.Id == taskInfo.TaskId);
                    var sessionTask = session.SessionTasks.FirstOrDefault(st => st.TaskId == taskInfo.TaskId);

                    if (sessionTask != null)
                    {
                        sessionTask.EndTimeUtc = taskInfo.EndTime;
                        
                        // Nếu EndTime khác null -> Completed
                        if (taskInfo.EndTime.HasValue)
                        {
                            sessionTask.Status = "COMPLETED";
                            if (task != null)
                            {
                                task.Status = Domain.Enums.TaskStatus.Completed;
                                task.CompletedAt = taskInfo.EndTime;
                            }
                            tasksCompletedCount++;
                        }
                    }
                }
            }

            // Save counts to session entity
            session.TasksCompletedCount = tasksCompletedCount;
            session.TotalTasks = totalTasks; // Ensures it's explicitly set if it was missed

            // Calculate XP: floor(seconds/60) * 10 + tasksCompleted * 25
            var activeMinutes = (session.ActualDurationSeconds ?? 0) / 60;
            var xpEarned = activeMinutes * 10 + tasksCompletedCount * 25;

            await context.SaveChangesAsync(ct);

            return new EndSessionResult
            {
                Success = true,
                Message = "Session ended successfully",
                Data = new SessionSummaryResponse
                {
                    SessionId = session.Id,
                    TotalDurationMinutes = (session.ActualDurationSeconds ?? 0) / 60,
                    TasksCompleted = tasksCompletedCount,
                    TotalTasks = totalTasks,
                    XpEarned = xpEarned
                }
            };
        }
    }
}
