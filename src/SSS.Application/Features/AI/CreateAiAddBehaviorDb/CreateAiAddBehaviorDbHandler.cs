using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;
using System.Text.Json;

namespace SSS.Application.Features.AI.CreateAiAddBehaviorDb
{
    public class CreateAiAddBehaviorDbHandler(IPipeLine pipeLine, IAppDbContext db)
        : IRequestHandler<CreateAiAddBehaviorDbCommand, CreateAiAddBehaviorDbResult>
    {
        public async Task<CreateAiAddBehaviorDbResult> Handle(CreateAiAddBehaviorDbCommand req, CancellationToken ct)
        {
            long? studyPlanId = null;
            if (long.TryParse(req.StudyplanmoduleId, out var parsedStudyPlanId))
            {
                studyPlanId = parsedStudyPlanId;
            }

            var module = await db.StudyPlanModules
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == studyPlanId, ct);

            var sessionsQuery = db.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                        .ThenInclude(t => t.StudyPlanModule)
                .Where(s => s.UserId == req.UserId && s.Status == SessionStatus.Completed);

            if (studyPlanId.HasValue)
            {
                sessionsQuery = sessionsQuery.Where(s => s.SessionTasks
                    .Any(st => st.TaskItem.StudyPlanModule.StudyPlanId == studyPlanId.Value));
            }

            var sessions = await sessionsQuery
                .OrderByDescending(s => s.EndAt ?? s.StartAt)
                .Take(20)
                .ToListAsync(ct);

            var behaviorContext = sessions.Select(s => new
            {
                s.Id,
                s.StartAt,
                s.EndAt,
                s.Status,
                s.TasksCompletedCount,
                s.TotalTasks,
                s.FocusScore,
                s.FatigueScore,
                s.SelfRating,
                Tasks = s.SessionTasks.Select(st => new
                {
                    st.TaskId,
                    SessionTaskStatus = st.Status,
                    st.StartTimeUtc,
                    st.EndTimeUtc,
                    TaskTitle = st.TaskItem.Title,
                    TaskStatus = st.TaskItem.Status,
                    st.TaskItem.ScheduledDate,
                    st.TaskItem.CompletedAt,
                    st.TaskItem.EstimatedDurationSeconds,
                    StudyPlanId = st.TaskItem.StudyPlanModule.StudyPlanId
                }).ToList()
            }).ToList();

            var behaviorContextJson = JsonSerializer.Serialize(behaviorContext);

            var result = await pipeLine.GenerateBehaviorResultAsync(behaviorContextJson, ct);

            if (result is null)
            {
                throw new Exception("Failed to generate behavior result.");
            }

            var chunks = new List<(string Text, string? Source)>
            {
                (result, "user_behavior")
            };

            await pipeLine.IngestBehaviorAsync(req.StudyplanId, req.UserId, req.StudyplanmoduleId, chunks, ct);

            return new CreateAiAddBehaviorDbResult
            {
                Success = true,
                Message = "Behavior added to the database successfully."
            };
        }
    }
}
