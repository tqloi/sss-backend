using Hangfire;
using SSS.Application.Abstractions.Background;
using SSS.Infrastructure.Background.Jobs;

namespace SSS.Infrastructure.Background
{
    /// <summary>
    /// Hangfire-backed dispatcher for module jobs.
    /// </summary>
    public class HangfireModuleJobDispatcher : IModuleJobDispatcher
    {
        public void DispatchCompleteModule(int moduleId)
        {
            BackgroundJob.Enqueue<CompleteModuleJob>(
                j => j.ExecuteAsync(moduleId, CancellationToken.None));
        }
    }
}
