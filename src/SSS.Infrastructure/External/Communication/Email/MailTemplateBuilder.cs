using SSS.Application.Abstractions.External.Communication.Email;
using System.Reflection;

namespace SSS.Infrastructure.External.Communication.Email
{
    public class MailTemplateBuilder : IMailTemplateBuilder
    {
        // ===== Template file names (EmbeddedResource) =====
        private const string TEMPLATE_CONFIRM = "ConfirmEmailTemplate.html";
        private const string TEMPLATE_RESET_PASSWORD = "ResetPasswordTemplate.html";
        private const string TEMPLATE_WELCOME_COURSE = "WelcomeCourseTemplate.html";
        private const string TEMPLATE_COMPLETION_COURSE = "CourseCompletedTemplate.html";
        private const string TEMPLATE_OTP = "OtpEmailTemplate.html";

        // ===== Placeholder keys =====
        private const string PH_CONFIRMATION_URL = "{{ .ConfirmationURL }}";
        private const string PH_EMAIL = "{{ .Email }}";
        private const string PH_STUDENT_NAME = "{{ .StudentName }}";
        private const string PH_COURSE_NAME = "{{ .CourseName }}";
        private const string PH_COURSE_URL = "{{ .CourseUrl }}";
        private const string PH_CERT_URL = "{{ .CertificateUrl }}";
        private const string PH_OTP_CODE = "{{ .OtpCode }}";

        // ===== Public APIs =====

        // Confirm email
        public Task<string> BuildConfirmEmailAsync(string confirmationUrl, string email) =>
            BuildLinkActionEmailAsync(TEMPLATE_CONFIRM, confirmationUrl, email);

        // Reset password
        public Task<string> BuildResetPasswordEmailAsync(string resetUrl, string email) =>
            BuildLinkActionEmailAsync(TEMPLATE_RESET_PASSWORD, resetUrl, email);

        // Welcome to course (sau khi mua)
        public async Task<string> BuildWelcomeToCourseEmailAsync(string studentName, string courseName, string courseUrl, string email)
        {
            var html = await LoadTemplateAsync(TEMPLATE_WELCOME_COURSE);

            html = html
                .Replace(PH_STUDENT_NAME, studentName ?? string.Empty)
                .Replace(PH_COURSE_NAME, courseName ?? string.Empty)
                .Replace(PH_COURSE_URL, courseUrl ?? string.Empty)
                .Replace(PH_EMAIL, email ?? string.Empty);

            return html;
        }

        // Course completed (kèm link certificate)
        public async Task<string> BuildCourseCompletedEmailAsync(string studentName, string courseName, string certificateUrl, string email)
        {
            var html = await LoadTemplateAsync(TEMPLATE_COMPLETION_COURSE);

            html = html
                .Replace(PH_STUDENT_NAME, studentName ?? string.Empty)
                .Replace(PH_COURSE_NAME, courseName ?? string.Empty)
                .Replace(PH_CERT_URL, certificateUrl ?? string.Empty)
                .Replace(PH_EMAIL, email ?? string.Empty);

            return html;
        }

        // ===== Private helpers =====

        // Dùng chung cho Confirm/Reset: template có {{ .ConfirmationURL }} và {{ .Email }}
        private async Task<string> BuildLinkActionEmailAsync(string templateFileName, string actionUrl, string email)
        {
            var html = await LoadTemplateAsync(templateFileName);

            html = html
                .Replace(PH_CONFIRMATION_URL, actionUrl ?? string.Empty)
                .Replace(PH_EMAIL, email ?? string.Empty);

            return html;
        }

        // Đọc EmbeddedResource theo namespace hiện tại
        private static async Task<string> LoadTemplateAsync(string templateFileName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourcePath = $"{typeof(MailTemplateBuilder).Namespace}.Templates.{templateFileName}";

            await using var stream = asm.GetManifestResourceStream(resourcePath)
                ?? throw new FileNotFoundException($"Email template not found: {resourcePath}");

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }

        public async Task<string> BuildSendOtpEmailAsync(string otpCode, string email)
        {
            var html = await LoadTemplateAsync(TEMPLATE_OTP);

            html = html
                .Replace(PH_OTP_CODE, otpCode ?? string.Empty)
                .Replace(PH_EMAIL, email ?? string.Empty);

            return html;
        }
    }
}
