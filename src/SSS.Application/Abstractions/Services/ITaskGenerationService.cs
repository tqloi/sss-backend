namespace SSS.Application.Abstractions.Services
{
    public interface ITaskGenerationService
    {
        /// <summary>
        /// Loads the StudyPlan with its modules + learner profile,
        /// calls AI to generate tasks for every module,
        /// and persists the resulting TaskItems to the database.
        /// Does NOT update StudyPlan.Status — the caller (job) handles that.
        /// </summary>
        Task GenerateAndSaveTasksAsync(long studyPlanId, CancellationToken ct = default);
    }
}
