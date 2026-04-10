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
                .Include(m => m.RoadmapNode)
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

            var scopedNodeIds = new List<long> { module.RoadmapNodeId };
            var cursorNodeId = module.RoadmapNodeId;

            for (var i = 0; i < 2; i++)
            {
                var previousNodeId = await db.RoadmapEdges
                    .AsNoTracking()
                    .Where(e => e.ToNodeId == cursorNodeId && e.EdgeType == EdgeType.Next)
                    .OrderByDescending(e => e.OrderNo ?? int.MinValue)
                    .ThenByDescending(e => e.Id)
                    .Select(e => (long?)e.FromNodeId)
                    .FirstOrDefaultAsync(ct);

                if (!previousNodeId.HasValue || scopedNodeIds.Contains(previousNodeId.Value))
                    break;

                scopedNodeIds.Add(previousNodeId.Value);
                cursorNodeId = previousNodeId.Value;
            }

            var nearbyModules = await db.StudyPlanModules
                .AsNoTracking()
                .Include(m => m.RoadmapNode)
                .Include(m => m.Tasks)
                .Where(m => m.StudyPlanId == module.StudyPlanId && scopedNodeIds.Contains(m.RoadmapNodeId))
                .ToListAsync(ct);

            nearbyModules = nearbyModules
                .OrderBy(m =>
                {
                    var index = scopedNodeIds.IndexOf(m.RoadmapNodeId);
                    return index < 0 ? int.MaxValue : index;
                })
                .ThenBy(m => m.Id)
                .Take(3)
                .ToList();

            if (nearbyModules.Count == 0)
                nearbyModules = [module];

            var nearbyModuleIds = nearbyModules.Select(m => m.Id).ToHashSet();
            var nearbyNodeIds = nearbyModules.Select(m => m.RoadmapNodeId).ToHashSet();

            var quizAttempts = await db.QuizAttempts
                .AsNoTracking()
                .Include(a => a.Quiz)
                .Include(a => a.Answers)
                .Where(a => a.UserId == req.UserId && nearbyNodeIds.Contains(a.Quiz.RoadmapNodeId))
                .OrderByDescending(a => a.SubmittedAt ?? a.StartedAt)
                .Take(20)
                .ToListAsync(ct);

            var sessions = await db.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                .Where(s => s.UserId == req.UserId &&
                            ((s.StudyPlanModuleId.HasValue && nearbyModuleIds.Contains(s.StudyPlanModuleId.Value)) ||
                             s.SessionTasks.Any(st => nearbyModuleIds.Contains(st.TaskItem.StudyPlanModuleId))))
                .OrderByDescending(s => s.EndAt ?? s.StartAt)
                .Take(20)
                .ToListAsync(ct);

            var completedTaskCount = nearbyModules
                .SelectMany(m => m.Tasks)
                .Count(t => t.Status == SSS.Domain.Enums.TaskStatus.Completed || t.CompletedAt.HasValue);
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
            var nearbyModuleDtos = mapper.Map<List<BehaviorModuleDto>>(nearbyModules);
            var sessionDtos = mapper.Map<List<BehaviorSessionDto>>(sessions);
            foreach (var sessionDto in sessionDtos)
            {
                sessionDto.Tasks = sessionDto.Tasks
                    .Where(t => nearbyModuleIds.Contains(t.StudyPlanModuleId))
                    .ToList();
            }
            var quizAttemptDtos = mapper.Map<List<BehaviorQuizAttemptDto>>(quizAttempts);

            var behaviorContext = new
            {
                NodeScope = new
                {
                    CurrentModuleId = module.Id,
                    ScopedNodeIds = scopedNodeIds,
                    NearbyModuleIds = nearbyModuleIds,
                    NearbyModules = nearbyModuleDtos
                },
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
