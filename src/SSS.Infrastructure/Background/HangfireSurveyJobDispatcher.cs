using Hangfire;
using SSS.Application.Abstractions.Background;
using SSS.Infrastructure.Background.Jobs;

namespace SSS.Infrastructure.Background
{
    /// <summary>
    /// Hangfire-backed implementation of ISurveyJobDispatcher.
    /// Registered as Scoped; Hangfire creates a new DI scope per job execution.
    /// </summary>
    public class HangfireSurveyJobDispatcher : ISurveyJobDispatcher
    {
        public void DispatchBehaviorAnalysis(long responseId)
        {
            BackgroundJob.Enqueue<AnalyzeBehaviorJob>(
                j => j.ExecuteAsync(responseId, CancellationToken.None));
        }

        public void DispatchTargetAnalysis(long responseId, long roadmapId)
        {
            BackgroundJob.Enqueue<AnalyzeTargetJob>(
                j => j.ExecuteAsync(responseId, roadmapId, CancellationToken.None));
        }

        public void DispatchModuleBehaviorInsight(long studyPlanId, int moduleId, string userId)
        {
            BackgroundJob.Enqueue<AnalyzeModuleBehaviorInsightJob>(
                j => j.ExecuteAsync(studyPlanId, moduleId, userId, CancellationToken.None));
        }
    }
}
