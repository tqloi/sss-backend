using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Services
{
    public class StudyPlanService(
        IAppDbContext db,
        ILogger<StudyPlanService> logger) : IStudyPlanService
    {
        public async Task<StudyPlan> CreatePlanWithModulesAsync(
            string userId, long roadmapId, CancellationToken ct = default)
        {
            // Load all nodes of the roadmap ordered by their sequence
            var nodes = await db.RoadmapNodes
                .Where(n => n.RoadmapId == roadmapId)
                .OrderBy(n => n.OrderNo)
                .ToListAsync(ct);

            if (nodes.Count == 0)
                logger.LogWarning("[StudyPlanService] Roadmap {RoadmapId} has no nodes. Creating plan without modules.", roadmapId);

            // Create the plan — starts in GeneratingTasks so the frontend can show a loading state
            var plan = new StudyPlan
            {
                UserId = userId,
                RoadmapId = roadmapId,
                ProfileVersion = 1,
                Strategy = StudyPlanStrategy.Balanced,
                Status = StudyPlanStatus.Ready,
                CreatedAt = DateTime.UtcNow
            };

            db.StudyPlans.Add(plan);
            await db.SaveChangesAsync(ct);

            // One module per roadmap node — all locked until tasks are generated
            if (nodes.Count > 0)
            {
                var modules = nodes.Select(node => new StudyPlanModule
                {
                    StudyPlanId = plan.Id,
                    RoadmapNodeId = node.Id,
                    Status = ModuleStatus.Locked,
                    isTaskGenerated = false
                }).ToList();

                db.StudyPlanModules.AddRange(modules);
                await db.SaveChangesAsync(ct);
            }

            logger.LogInformation("[StudyPlanService] StudyPlan {PlanId} created for user {UserId} with {ModuleCount} modules.",
                plan.Id, userId, nodes.Count);

            return plan;
        }

        public async Task SetStatusAsync(long studyPlanId, StudyPlanStatus status, CancellationToken ct = default)
        {
            var plan = await db.StudyPlans.FindAsync(new object[] { studyPlanId }, ct);
            if (plan is null)
            {
                logger.LogWarning("[StudyPlanService] StudyPlan {PlanId} not found when setting status {Status}.", studyPlanId, status);
                return;
            }

            plan.Status = status;
            await db.SaveChangesAsync(ct);

            logger.LogInformation("[StudyPlanService] StudyPlan {PlanId} status → {Status}.", studyPlanId, status);
        }
    }
}
