namespace SSS.Application.Abstractions.External.Communication.Email
{
    public interface ISmtpEmailSender
    {
        Task SendMailAsync(EmailContent mailContent);
    }
}
