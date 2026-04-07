using Hangfire;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SSS.Application.Common.Exceptions;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Features.AI.Common;

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
        IPipeLine pipeLine,
        IMapper mapper,
        IAppDbContext db,
        ILogger<AnalyzeTargetJob> logger)
    {
        [AutomaticRetry(Attempts = 0, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
        public async Task ExecuteAsync(long responseId, long roadmapId, CancellationToken ct = default)
        {
            logger.LogInformation("[AnalyzeTargetJob] Starting for responseId={ResponseId} roadmapId={RoadmapId}", responseId, roadmapId);

            // 1. AI analysis → UserLearningTarget
            var target = await surveyAnalysis.AnalyzeTargetAsync(responseId, ct);
            target.RoadmapId = roadmapId;

            //var roadmapTitle = await db.Roadmaps
            //    .Where(x => x.Id == roadmapId)
            //    .Select(x => x.Title)
            //    .FirstOrDefaultAsync();
            //target.TargetRole = target.TargetRole + roadmapTitle;

            db.UserLearningTargets.Add(target);
            await db.SaveChangesAsync(ct);

            logger.LogInformation("[AnalyzeTargetJob] UserLearningTarget saved for userId={UserId}", target.UserId);

            // 2. Create StudyPlan + one module per RoadmapNode (status = GeneratingTasks)
            // Handle known business conflicts (e.g. roadmap limit reached) without throwing,
            // so one failed business case does not trigger repeated AI calls.
            var plan = default(SSS.Domain.Entities.Planning.StudyPlan);
            try
            {
                plan = await studyPlanService.CreatePlanWithModulesAsync(target.UserId, roadmapId, ct);
            }
            catch (ConflictException ex)
            {
                logger.LogWarning(ex,
                    "[AnalyzeTargetJob] Skipping plan creation due to business conflict. ResponseId={ResponseId}, RoadmapId={RoadmapId}, UserId={UserId}",
                    responseId,
                    roadmapId,
                    target.UserId);
                return;
            }

            // Ingest QDrant
            var behavior = await db.UserLearningBehaviors
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.UserId == target.UserId, ct);

            var studyplanId = await db.StudyPlans
                .Where(x =>
                    x.UserId == target.UserId &&
                    x.RoadmapId == target.RoadmapId)
                .OrderByDescending(x => x.CreatedAt)
                .Select(s => s.Id)
                .FirstOrDefaultAsync();

            if (behavior is null)
            {
                logger.LogWarning("[AnalyzeTargetJob] UserLearningBehavior not found for userId={UserId}. Skipping profile ingestion.", target.UserId);
            }
            else
            {
                var behaviorDto = mapper.Map<UserLearningBehaviorDto>(behavior);
                var targetDto = mapper.Map<UserLearningTargetDto>(target);

                var surveyResult = await pipeLine.GenerateSurveyResultAsync(targetDto, behaviorDto, ct);

                if (surveyResult is null)
                {
                    throw new Exception("Failed to generate survey result.");
                }

                var chunks = new List<(string Text, string? Source)>
                {
                    (surveyResult, "user_profile")
                };

                await pipeLine.IngestAsync(
                    studyplanId.ToString(),
                    target.UserId,
                    chunks,
                    ct);
            }

            logger.LogInformation("[AnalyzeTargetJob] StudyPlan {PlanId} created with modules. Enqueueing GenerateTasksJob.", plan!.Id);

            // 3. Chain task-generation job — runs after this job succeeds
            //BackgroundJob.Enqueue<GenerateTasksJob>(j => j.ExecuteAsync(plan.Id, CancellationToken.None));
        }
    }
}
