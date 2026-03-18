using Hangfire;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;

namespace SSS.Infrastructure.Background.Jobs
{
    /// <summary>
    /// Background job: analyze a Learning Target survey via AI, persist UserLearningTarget,
    /// create StudyPlan + modules, then chain GenerateTasksJob.
    /// Triggered by: ISurveyJobDispatcher.DispatchTargetAnalysis
    /// </summary>
    public class AnalyzeTargetJob(
        ISurveyAnalysisService surveyAnalysis,
        IStudyPlanService studyPlanService,
        IAppDbContext db,
        ILogger<AnalyzeTargetJob> logger)
    {
        public async Task ExecuteAsync(long responseId, long roadmapId, CancellationToken ct = default)
        {
            logger.LogInformation("[AnalyzeTargetJob] Starting for responseId={ResponseId} roadmapId={RoadmapId}", responseId, roadmapId);

            // 1. AI analysis → UserLearningTarget
            var target = await surveyAnalysis.AnalyzeTargetAsync(responseId, ct);
            target.RoadmapId = roadmapId;

            db.UserLearningTargets.Add(target);
            await db.SaveChangesAsync(ct);

            logger.LogInformation("[AnalyzeTargetJob] UserLearningTarget saved for userId={UserId}", target.UserId);

            // 2. Create StudyPlan + one module per RoadmapNode (status = GeneratingTasks)
            var plan = await studyPlanService.CreatePlanWithModulesAsync(target.UserId, roadmapId, ct);

            logger.LogInformation("[AnalyzeTargetJob] StudyPlan {PlanId} created with modules. Enqueueing GenerateTasksJob.", plan.Id);

            // 3. Chain task-generation job — runs after this job succeeds
            BackgroundJob.Enqueue<GenerateTasksJob>(j => j.ExecuteAsync(plan.Id, CancellationToken.None));
        }
    }
}
