using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Background.Jobs
{
    /// <summary>
    /// Background job: generate AI tasks for every module of a StudyPlan,
    /// then mark the plan as Ready (or Failed on error).
    /// Triggered by: AnalyzeTargetJob (chained via Hangfire)
    /// </summary>
    public class GenerateTasksJob(
        ITaskGenerationService taskGenerationService,
        IStudyPlanService studyPlanService,
        ILogger<GenerateTasksJob> logger)
    {
        public async Task ExecuteAsync(long studyPlanId, CancellationToken ct = default)
        {
            logger.LogInformation("[GenerateTasksJob] Starting for studyPlanId={StudyPlanId}", studyPlanId);

            try
            {
                await taskGenerationService.GenerateAndSaveTasksAsync(studyPlanId, ct);
                await studyPlanService.SetStatusAsync(studyPlanId, StudyPlanStatus.Ready, ct);

                logger.LogInformation("[GenerateTasksJob] Completed. StudyPlan {StudyPlanId} is now Ready.", studyPlanId);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[GenerateTasksJob] Failed for studyPlanId={StudyPlanId}. Marking as Failed.", studyPlanId);

                // Best-effort status update — do not throw from here so the plan stays Failed
                try { await studyPlanService.SetStatusAsync(studyPlanId, StudyPlanStatus.Failed, CancellationToken.None); }
                catch (Exception statusEx)
                {
                    logger.LogError(statusEx, "[GenerateTasksJob] Could not set Failed status for studyPlanId={StudyPlanId}", studyPlanId);
                }

                throw; // Let Hangfire record the failure and retry according to policy
            }
        }
    }
}
