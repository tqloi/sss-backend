using Hangfire;
using SSS.Application.Abstractions.Background;
using SSS.Infrastructure.Background.Jobs;

namespace SSS.Infrastructure.Background
{
    /// <summary>
    /// Hangfire-backed dispatcher for email jobs.
    /// </summary>
    public class HangfireEmailJobDispatcher : IEmailJobDispatcher
    {
        public void DispatchSendEmail(string to, string subject, string body)
        {
            BackgroundJob.Enqueue<SendEmailJob>(
                job => job.ExecuteAsync(to, subject, body, CancellationToken.None));
        }
    }
}