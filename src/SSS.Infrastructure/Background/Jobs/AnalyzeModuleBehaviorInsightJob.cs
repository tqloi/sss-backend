using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Background.Jobs
{
    /// <summary>
    /// Background job: load latest user profile/behavior points from Qdrant,
    /// generate a concise AI insight, then notify the user.
    /// Triggered after module behavior ingestion.
    /// </summary>
    public class AnalyzeModuleBehaviorInsightJob(
        IPipeLine pipeLine,
        INotificationService notificationService,
        ILogger<AnalyzeModuleBehaviorInsightJob> logger)
    {
        public async Task ExecuteAsync(long studyPlanId, int moduleId, string userId, CancellationToken ct = default)
        {
            logger.LogInformation(
                "[AnalyzeModuleBehaviorInsightJob] Starting. StudyPlanId={StudyPlanId}, ModuleId={ModuleId}, UserId={UserId}",
                studyPlanId,
                moduleId,
                userId);

            var context = await pipeLine.BuildStudyPlanContextAsync(userId, studyPlanId.ToString(), ct);

            if (string.IsNullOrWhiteSpace(context))
            {
                logger.LogWarning(
                    "[AnalyzeModuleBehaviorInsightJob] Empty context. Skip notify. StudyPlanId={StudyPlanId}, ModuleId={ModuleId}, UserId={UserId}",
                    studyPlanId,
                    moduleId,
                    userId);
                return;
            }

            var prompt = $"""
You are an assistant that writes a very simple learning summary for users.
Use the context to summarize module/node {moduleId}.

Output rules:
- Plain text only.
- Maximum 3 short sentences.
- Very easy to understand.
- Include only these fields:
  1) Total study time spent.
  2) Quiz score/result.
  3) Basic evaluation (Good / Average / Need improvement).
- If any field is missing in context, say "No data" for that field.
- Do not include technical terms or extra advice.

Context:
{context}
""";

            var insight = await pipeLine.AskAsync(prompt, ct);

            if (string.IsNullOrWhiteSpace(insight))
            {
                logger.LogWarning(
                    "[AnalyzeModuleBehaviorInsightJob] Empty insight. Skip notify. StudyPlanId={StudyPlanId}, ModuleId={ModuleId}, UserId={UserId}",
                    studyPlanId,
                    moduleId,
                    userId);
                return;
            }

            await notificationService.CreateAndDispatchAsync(
                userId: userId,
                title: "Phan tich hoc tap moi da san sang",
                content: insight,
                type: NotificationType.AiRecommendation,
                relatedType: NotificationRelatedType.Module,
                relatedId: moduleId,
                ct: ct);

            logger.LogInformation(
                "[AnalyzeModuleBehaviorInsightJob] Completed. StudyPlanId={StudyPlanId}, ModuleId={ModuleId}, UserId={UserId}",
                studyPlanId,
                moduleId,
                userId);
        }
    }
}
