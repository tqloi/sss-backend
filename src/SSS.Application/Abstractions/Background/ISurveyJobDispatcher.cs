namespace SSS.Application.Abstractions.Background
{
    /// <summary>
    /// Dispatches background jobs triggered by survey submission events.
    /// Implemented in Infrastructure using Hangfire.
    /// </summary>
    public interface ISurveyJobDispatcher
    {
        /// <summary>
        /// Enqueues a job to analyze a Learning Behavior survey response via AI
        /// and persist the resulting UserLearningBehavior profile.
        /// </summary>
        void DispatchBehaviorAnalysis(long responseId);

        /// <summary>
        /// Enqueues a job to analyze a Learning Target survey response via AI,
        /// persist UserLearningTarget, create a StudyPlan with modules,
        /// and chain a task-generation job for that plan.
        /// </summary>
        void DispatchTargetAnalysis(long responseId, long roadmapId);
    }
}
