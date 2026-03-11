using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Entities.Tracking;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudySessions.StartSession
{
    public class StartSessionHandler(IAppDbContext context)
        : IRequestHandler<StartSessionCommand, StartSessionResult>
    {
        public async Task<StartSessionResult> Handle(StartSessionCommand req, CancellationToken ct)
        {
            // Business rule: user không được có session đang InProgress/Paused
            var hasActive = await context.StudySessions.AnyAsync(
                s => s.UserId == req.UserId &&
                     (s.Status == SessionStatus.InProgress || s.Status == SessionStatus.Paused), ct);

            if (hasActive)
                throw new ConflictException("User already has an active session. Please end it before starting a new one.");

            // Generate ID
            var sessionId = ObjectId.GenerateNewId();

            // Snapshot total tasks at the start of the session
            int initialTotalTasks = 0;
            if (req.ModuleId.HasValue)
            {
                initialTotalTasks = await context.TaskItems
                    .CountAsync(t => t.StudyPlanModuleId == req.ModuleId, ct);
            }
            // else if other levels like TaskId, we can expand later

            var session = new StudySession
            {
                Id = sessionId,
                UserId = req.UserId,
                StudyPlanId = req.StudyPlanId,
                NodeId = req.NodeId,
                ModuleId = req.ModuleId,
                TaskId = req.TaskId,
                PlannedDurationSeconds = req.PlannedDurationSeconds,
                Timezone = req.Timezone,
                Status = SessionStatus.InProgress,
                StartAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                TasksCompletedCount = 0,
                TotalTasks = initialTotalTasks
            };

            context.StudySessions.Add(session);
            await context.SaveChangesAsync(ct);

            // Load related data for response
            var savedSession = await context.StudySessions
                .AsNoTracking()
                .Include(s => s.Node)
                .Include(s => s.Task)
                .FirstAsync(s => s.Id == sessionId, ct);

            // Load tasks of the module/node if available
            var tasks = new List<SessionTaskDto>();
            if (req.ModuleId.HasValue)
            {
                tasks = await context.TaskItems
                    .AsNoTracking()
                    .Where(t => t.StudyPlanModuleId == req.ModuleId)
                    .Select(t => new SessionTaskDto
                    {
                        Id = t.Id,
                        Title = t.Title ?? "",
                        Description = t.Description,
                        Order = 0,
                        EstimatedMinutes = t.EstimatedDurationSeconds / 60,
                        IsCompleted = t.Status == Domain.Enums.TaskStatus.Completed
                    })
                    .ToListAsync(ct);
            }

            return new StartSessionResult
            {
                Success = true,
                Message = "Session started successfully",
                Data = new StartSessionResponse
                {
                    SessionId = sessionId,
                    StartAt = session.StartAt,
                    Status = session.Status.ToString(),
                    Node = savedSession.Node != null ? new SessionNodeDto
                    {
                        Id = savedSession.Node.Id,
                        Title = savedSession.Node.Title ?? "",
                        Description = savedSession.Node.Description
                    } : null,
                    Tasks = tasks
                }
            };
        }
    }

    // Simple ObjectId generator (24-char hex string)
    internal static class ObjectId
    {
        public static string GenerateNewId()
        {
            var timestamp = BitConverter.GetBytes((int)(DateTimeOffset.UtcNow.ToUnixTimeSeconds()));
            if (BitConverter.IsLittleEndian) Array.Reverse(timestamp);
            var random = new byte[8];
            Random.Shared.NextBytes(random);
            var bytes = new byte[12];
            Array.Copy(timestamp, 0, bytes, 0, 4);
            Array.Copy(random, 0, bytes, 4, 8);
            return Convert.ToHexString(bytes).ToLowerInvariant();
        }
    }
}
