using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.External.Communication.Email;

namespace SSS.Infrastructure.Background.Jobs
{
    /// <summary>
    /// Background job for sending a single email via the configured SMTP sender.
    /// </summary>
    public class SendEmailJob(
        ISmtpEmailSender emailSender,
        ILogger<SendEmailJob> logger)
    {
        public async Task ExecuteAsync(string to, string subject, string body, CancellationToken ct = default)
        {
            logger.LogInformation("[SendEmailJob] Starting email send to {To}", to);

            await emailSender.SendMailAsync(new EmailContent
            {
                To = to,
                Subject = subject,
                Body = body
            });

            logger.LogInformation("[SendEmailJob] Completed email send to {To}", to);
        }
    }
}