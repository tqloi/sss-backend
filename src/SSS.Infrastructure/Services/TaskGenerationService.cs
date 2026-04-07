using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Planning;
using System.Text.Json;

namespace SSS.Infrastructure.Services
{
    public class TaskGenerationService(
        IAppDbContext db,
        IPipeLine pipeLine,
        ILogger<TaskGenerationService> logger) : ITaskGenerationService
    {
        public async Task GenerateAndSaveTasksAsync(long studyPlanId, CancellationToken ct = default)
        {
            logger.LogInformation("[TaskGenerationService] Loading data for StudyPlan {PlanId}", studyPlanId);

            // Load plan + modules + roadmap nodes
            var plan = await db.StudyPlans
                .Include(p => p.Modules)
                    .ThenInclude(m => m.RoadmapNode)
                .FirstOrDefaultAsync(p => p.Id == studyPlanId, ct)
                ?? throw new InvalidOperationException($"StudyPlan {studyPlanId} not found");

            // Load latest behavior for scheduling context
            var behavior = await db.UserLearningBehaviors
                .Where(b => b.UserId == plan.UserId)
                .OrderByDescending(b => b.SnapshotAt)
                .FirstOrDefaultAsync(ct);

            var allTasks = new List<TaskItem>();
            var scheduleDate = DateTime.UtcNow.Date.AddDays(1);

            // Only generate tasks for the first module (lowest OrderNo)
            var modules = plan.Modules.OrderBy(m => m.RoadmapNode?.OrderNo ?? 0).Take(1).ToList();

            // Full generation — uncomment to generate tasks for ALL modules
            // var modules = plan.Modules.OrderBy(m => m.RoadmapNode?.OrderNo ?? 0).ToList();

            foreach (var module in modules)
            {
                logger.LogInformation("[TaskGenerationService] Generating tasks for first module {ModuleId} (node: {NodeTitle})",
                    module.Id, module.RoadmapNode?.Title);

                // Load roadmap + node separately (same as CreateAiTaskItemsHandler)
                var roadmapNode = await db.RoadmapNodes
                    .AsNoTracking()
                    .FirstOrDefaultAsync(n => n.Id == module.RoadmapNodeId, ct)
                    ?? throw new InvalidOperationException($"RoadmapNode {module.RoadmapNodeId} not found");

                var roadmap = await db.Roadmaps
                    .AsNoTracking()
                    .FirstOrDefaultAsync(r => r.Id == roadmapNode.RoadmapId, ct)
                    ?? throw new InvalidOperationException($"Roadmap {roadmapNode.RoadmapId} not found");

                var roadmapJson     = JsonSerializer.Serialize(roadmap);
                var roadmapNodeJson = JsonSerializer.Serialize(roadmapNode);

                // Call AI pipeline — same call as CreateAiTaskItemsHandler
                var aiResponse = await pipeLine.GenerateStudyPlanAsync(
                    plan.UserId,
                    module.StudyPlanId.ToString(),
                    roadmapJson,
                    roadmapNodeJson,
                    ct);

                if (string.IsNullOrWhiteSpace(aiResponse))
                {
                    logger.LogWarning("[TaskGenerationService] Empty AI response for module {ModuleId}", module.Id);
                    continue;
                }

                // Strip markdown fences (same as CreateAiTaskItemsHandler)
                aiResponse = aiResponse
                    .Replace("```json", "")
                    .Replace("```", "")
                    .Trim();

                // Parse and build TaskItems
                var moduleTasks = ParseModuleTasks(aiResponse, module.Id, behavior, ref scheduleDate);
                allTasks.AddRange(moduleTasks);

                module.isTaskGenerated = true;
            }

            if (allTasks.Count == 0)
            {
                logger.LogWarning("[TaskGenerationService] AI returned 0 tasks for StudyPlan {PlanId}", studyPlanId);
                return;
            }

            db.TaskItems.AddRange(allTasks);
            await db.SaveChangesAsync(ct);

            logger.LogInformation("[TaskGenerationService] Saved {Count} tasks for StudyPlan {PlanId}", allTasks.Count, studyPlanId);
        }

        /// <summary>
        /// Parses AI JSON output for a single module into TaskItem entities.
        /// Advances scheduleDate by one day per task.
        /// </summary>
        private List<TaskItem> ParseModuleTasks(
            string aiJson,
            long moduleId,
            Domain.Entities.Learning.UserLearningBehavior? behavior,
            ref DateTime scheduleDate)
        {
            var tasks = new List<TaskItem>();
            var defaultSec = (behavior?.SessionLengthPrefMinutes ?? 60) * 60;

            try
            {
                using var doc = JsonDocument.Parse(aiJson);
                var root = doc.RootElement;

                // Support both { "tasks": [...] } and a root array
                JsonElement tasksArr;
                if (root.ValueKind == JsonValueKind.Array)
                    tasksArr = root;
                else if (!root.TryGetProperty("tasks", out tasksArr))
                {
                    logger.LogWarning("[TaskGenerationService] AI response for module {ModuleId} has no 'tasks' key.", moduleId);
                    return tasks;
                }

                foreach (var taskEl in tasksArr.EnumerateArray())
                {
                    var title = taskEl.TryGetProperty("title", out var t)
                        ? t.GetString() ?? "Study task"
                        : "Study task";

                    var description = taskEl.TryGetProperty("description", out var d)
                        ? d.GetString()
                        : null;

                    var estimatedSec = taskEl.TryGetProperty("estimatedDurationSeconds", out var e)
                                    && e.ValueKind == JsonValueKind.Number
                        ? e.GetInt32()
                        : defaultSec;

                    tasks.Add(new TaskItem
                    {
                        StudyPlanModuleId        = moduleId,
                        Title                    = title,
                        Description              = description,
                        EstimatedDurationSeconds = estimatedSec,
                        Status                   = Domain.Enums.TaskStatus.Pending,
                        ScheduledDate            = scheduleDate
                    });

                    scheduleDate = scheduleDate.AddDays(1);
                }
            }
            catch (JsonException ex)
            {
                logger.LogError(ex, "[TaskGenerationService] Failed to parse AI response for module {ModuleId}.", moduleId);
            }

            return tasks;
        }
    }
}
