using MediatR;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;
using System.Text.Json;

namespace SSS.Application.Features.AI.CreateAiAddBehaviorDb
{
    public class CreateAiAddBehaviorDbHandler(IPipeLine pipeLine, IAppDbContext db, IMapper mapper)
        : IRequestHandler<CreateAiAddBehaviorDbCommand, CreateAiAddBehaviorDbResult>
    {
        public async Task<CreateAiAddBehaviorDbResult> Handle(CreateAiAddBehaviorDbCommand req, CancellationToken ct)
        {
            if (!long.TryParse(req.StudyplanmoduleId, out var moduleId))
            {
                throw new ArgumentException("Invalid StudyplanmoduleId.");
            }

            long? studyPlanId = null;
            if (long.TryParse(req.StudyplanId, out var parsedStudyPlanId))
            {
                studyPlanId = parsedStudyPlanId;
            }

            var module = await db.StudyPlanModules
                .AsNoTracking()
                .Include(m => m.Tasks)
                .FirstOrDefaultAsync(m => m.Id == moduleId, ct);

            if (module is null)
            {
                throw new KeyNotFoundException($"StudyPlanModule with id {moduleId} not found.");
            }

            if (studyPlanId.HasValue && module.StudyPlanId != studyPlanId.Value)
            {
                throw new ArgumentException("StudyplanId does not match StudyplanmoduleId.");
            }

            var quizAttempts = await db.QuizAttempts
                .AsNoTracking()
                .Include(a => a.Quiz)
                .Include(a => a.Answers)
                .Where(a => a.UserId == req.UserId && a.Quiz.RoadmapNodeId == module.RoadmapNodeId)
                .OrderByDescending(a => a.SubmittedAt ?? a.StartedAt)
                .Take(20)
                .ToListAsync(ct);

            var sessions = await db.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                .Where(s => s.UserId == req.UserId &&
                            (s.StudyPlanModuleId == moduleId ||
                             s.SessionTasks.Any(st => st.TaskItem.StudyPlanModuleId == moduleId)))
                .OrderByDescending(s => s.EndAt ?? s.StartAt)
                .Take(20)
                .ToListAsync(ct);

            var completedTaskCount = module.Tasks.Count(t => t.Status == SSS.Domain.Enums.TaskStatus.Completed || t.CompletedAt.HasValue);
            var completedQuizCount = quizAttempts.Count(a => a.Status != QuizAttemptStatus.InProgress || a.SubmittedAt.HasValue);

            if (completedTaskCount == 0 || completedQuizCount == 0)
            {
                return new CreateAiAddBehaviorDbResult
                {
                    Success = false,
                    Message = "Insufficient completion data: user must complete at least one task and one quiz attempt for this module node."
                };
            }

            var moduleDto = mapper.Map<BehaviorModuleDto>(module);
            var sessionDtos = mapper.Map<List<BehaviorSessionDto>>(sessions);
            foreach (var sessionDto in sessionDtos)
            {
                sessionDto.Tasks = sessionDto.Tasks
                    .Where(t => t.StudyPlanModuleId == moduleId)
                    .ToList();
            }
            var quizAttemptDtos = mapper.Map<List<BehaviorQuizAttemptDto>>(quizAttempts);

            var behaviorContext = new
            {
                Module = moduleDto,
                Sessions = sessionDtos,
                QuizAttempts = quizAttemptDtos
            };

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

            var vectorStudyPlanId = studyPlanId?.ToString() ?? module.StudyPlanId.ToString();
            await pipeLine.IngestBehaviorAsync(vectorStudyPlanId, req.UserId, moduleId.ToString(), chunks, ct);

            return new CreateAiAddBehaviorDbResult
            {
                Success = true,
                Message = "Behavior added to the database successfully."
            };
        }
    }
}
