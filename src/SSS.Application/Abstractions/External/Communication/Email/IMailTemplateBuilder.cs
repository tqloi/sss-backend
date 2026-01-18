namespace SSS.Application.Abstractions.External.Communication.Email
{
    public interface IMailTemplateBuilder
    {
        Task<string> BuildConfirmEmailAsync(string confirmationUrl, string email);
        Task<string> BuildResetPasswordEmailAsync(string resetUrl, string email);
        Task<string> BuildWelcomeToCourseEmailAsync(string studentName, string courseName, string courseUrl, string email);
        Task<string> BuildCourseCompletedEmailAsync(string studentName, string courseName, string certificateUrl, string email);
        Task<string> BuildSendOtpEmailAsync(string otpCode, string email);
    }
}
