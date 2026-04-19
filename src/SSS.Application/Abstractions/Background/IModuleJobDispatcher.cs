namespace SSS.Application.Abstractions.Background
{
    /// <summary>
    /// Dispatches module-related background jobs.
    /// </summary>
    public interface IModuleJobDispatcher
    {
        /// <summary>
        /// Enqueues a job to complete a study-plan module and trigger follow-up actions.
        /// </summary>
        void DispatchCompleteModule(int moduleId);
    }
}
