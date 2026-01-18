using MailKit;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using SSS.Application.Abstractions.External.Communication.Email;

namespace SSS.Infrastructure.External.Communication.Email
{
    public class SmtpEmailSender : ISmtpEmailSender
    {
        private readonly EmailOptions _emailOptions;
        private readonly ILogger<MailService> _logger;

        public SmtpEmailSender(IOptions<EmailOptions> mailOptions, ILogger<MailService> logger)
        {
            _emailOptions = mailOptions.Value;
            _logger = logger;
        }

        public async Task SendMailAsync(EmailContent mailContent)
        {
            var email = new MimeMessage();
            email.Sender = new MailboxAddress(_emailOptions.DisplayName, _emailOptions.Mail);
            email.From.Add(new MailboxAddress(_emailOptions.DisplayName, _emailOptions.Mail));
            email.To.Add(MailboxAddress.Parse(mailContent.To));
            email.Subject = mailContent.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailContent.Body;
            email.Body = builder.ToMessageBody();

            // use SmtpClient of MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(_emailOptions.Host, _emailOptions.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_emailOptions.Mail, _emailOptions.Password);
                await smtp.SendAsync(email);
                _logger.LogInformation("Send email successfully to " + mailContent.To);
            }
            catch (Exception ex)
            {
                // Send failed, email's content will be saved as mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await email.WriteToAsync(emailsavefile);

                _logger.LogInformation("Sending mail error, save as - " + emailsavefile);
                _logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

            _logger.LogInformation("send mail to " + mailContent.To);
        }
    }
}
