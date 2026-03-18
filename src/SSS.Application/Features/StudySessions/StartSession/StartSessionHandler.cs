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

            var session = new StudySession
            {
                Id = sessionId,
                UserId = req.UserId,
                StudyPlanId = req.StudyPlanId,
                StudyPlanModuleId = req.ModuleId,
                PlannedDurationSeconds = req.PlannedDurationSeconds,
                Timezone = req.Timezone,
                Status = SessionStatus.InProgress,
                StartAt = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                TasksCompletedCount = 0,
                TotalTasks = 0 // will update later
            };

            // Create SessionTasks
            var sessionTasks = new List<SessionTask>();
            if (req.TaskIds is { Length: > 0 })
            {
                foreach (var tid in req.TaskIds)
                {
                    sessionTasks.Add(new SessionTask
                    {
                        StudySessionId = sessionId,
                        TaskId = tid,
                        Status = "INCOMPLETE"
                    });
                }
            }       
            

            session.SessionTasks = sessionTasks;
            session.TotalTasks = sessionTasks.Count;

            context.StudySessions.Add(session);
            await context.SaveChangesAsync(ct);

            // Return response details
            SessionNodeDto? nodeDto = null;
            if (req.NodeId.HasValue)
            {
                var node = await context.RoadmapNodes.FindAsync([req.NodeId.Value], ct);
                if (node != null)
                {
                    nodeDto = new SessionNodeDto
                    {
                        Id = node.Id,
                        Title = node.Title ?? "",
                        Description = node.Description
                    };
                }
            }

            var tasks = new List<SessionTaskDto>();
            var targetTaskIds = sessionTasks.Select(st => st.TaskId).ToList();
            if (targetTaskIds.Count > 0)
            {
                tasks = await context.TaskItems
                    .AsNoTracking()
                    .Where(t => targetTaskIds.Contains(t.Id))
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
                    Node = nodeDto,
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
