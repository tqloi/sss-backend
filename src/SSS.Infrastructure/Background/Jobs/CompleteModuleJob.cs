using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Background.Jobs
{
    /// <summary>
    /// Background job: complete module workflow and notify frontend.
    /// </summary>
    public class CompleteModuleJob(
        IModuleService moduleService,
        IAppDbContext db,
        INotificationService notificationService,
        ISurveyJobDispatcher surveyJobDispatcher,
        ILogger<CompleteModuleJob> logger)
    {
        public async Task ExecuteAsync(int moduleId, CancellationToken ct = default)
        {
            logger.LogInformation("[CompleteModuleJob] Starting for moduleId={ModuleId}", moduleId);

            await moduleService.CompleteModuleAsync(moduleId, ct);

            var moduleInfo = await db.StudyPlanModules
                .AsNoTracking()
                .Where(m => m.Id == moduleId)
                .Select(m => new
                {
                    m.Id,
                    m.StudyPlanId,
                    m.RoadmapNodeId,
                    ModuleName = m.RoadmapNode.Title,
                    UserId = m.StudyPlan.UserId
                })
                .FirstOrDefaultAsync(ct);

            if (moduleInfo is null)
            {
                logger.LogWarning("[CompleteModuleJob] Module not found after completion. ModuleId={ModuleId}", moduleId);
                return;
            }

            await notificationService.CreateAndDispatchAsync(
                userId: moduleInfo.UserId,
                title: "Module completed",
                content: $"You have completed module '{moduleInfo.ModuleName}'. Great progress!",
                type: NotificationType.Achievement,
                relatedType: NotificationRelatedType.Module,
                relatedId: moduleInfo.Id,
                status: ModuleStatus.Completed.ToString(),
                actionUrl: $"/study-plans/{moduleInfo.StudyPlanId}",
                dedupeKey: $"moduleCompleted:{moduleInfo.StudyPlanId}:{moduleInfo.Id}",
                isPush: false,
                ct: ct);

            surveyJobDispatcher.DispatchModuleBehaviorInsight(moduleInfo.StudyPlanId, checked((int)moduleInfo.Id), moduleInfo.UserId);

            logger.LogInformation(
                "[CompleteModuleJob] Completed for moduleId={ModuleId}, studyPlanId={StudyPlanId}, userId={UserId}",
                moduleInfo.Id,
                moduleInfo.StudyPlanId,
                moduleInfo.UserId);
        }
    }
}
