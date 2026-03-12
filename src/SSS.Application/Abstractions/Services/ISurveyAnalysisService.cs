using SSS.Domain.Entities.Learning;

namespace SSS.Application.Abstractions.Services
{
    /// <summary>
    /// Service for analyzing survey responses using AI
    /// </summary>
    public interface ISurveyAnalysisService
    {
        /// <summary>
        /// Analyzes a learning behavior survey response
        /// </summary>
        /// <param name="responseId">The survey response ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>UserLearningBehavior object with analyzed data</returns>
        Task<UserLearningBehavior> AnalyzeBehaviorAsync(long responseId, CancellationToken ct = default);

        /// <summary>
        /// Analyzes a learning target survey response
        /// </summary>
        /// <param name="responseId">The survey response ID</param>
        /// <param name="ct">Cancellation token</param>
        /// <returns>UserLearningTarget object with analyzed data</returns>
        Task<UserLearningTarget> AnalyzeTargetAsync(long responseId, CancellationToken ct = default);
    }
}
