using SSS.Application.Abstractions.External.Communication.Email;
using System.Reflection;

namespace SSS.Infrastructure.External.Communication.Email
{
    public class MailTemplateBuilder : IMailTemplateBuilder
    {
        private const string ConfirmTemplate = "ConfirmEmailTemplate.html";
        private const string ResetPasswordTemplate = "ResetPasswordTemplate.html";
        private const string WelcomeCourseTemplate = "WelcomeCourseTemplate.html";
        private const string CourseCompletedTemplate = "CourseCompletedTemplate.html";
        private const string OtpTemplate = "OtpEmailTemplate.html";
        private const string PlanReadyTemplate = "PlanReadyTemplate.html";
        private const string PremiumUpgradeTemplate = "PremiumUpgradeTemplate.html";
        private const string ModuleCompletedTemplate = "ModuleCompletedTemplate.html";

        private const string PhConfirmationUrl = "{{ .ConfirmationURL }}";
        private const string PhEmail = "{{ .Email }}";
        private const string PhStudentName = "{{ .StudentName }}";
        private const string PhCourseName = "{{ .CourseName }}";
        private const string PhCourseUrl = "{{ .CourseUrl }}";
        private const string PhCertificateUrl = "{{ .CertificateUrl }}";
        private const string PhOtpCode = "{{ .OtpCode }}";
        private const string PhPlanName = "{{ .PlanName }}";
        private const string PhRoadmapName = "{{ .RoadmapName }}";
        private const string PhPlanUrl = "{{ .PlanUrl }}";
        private const string PhPackageName = "{{ .PackageName }}";
        private const string PhInvoiceNumber = "{{ .InvoiceNumber }}";
        private const string PhInvoiceDate = "{{ .InvoiceDate }}";
        private const string PhInvoiceUrl = "{{ .InvoiceUrl }}";
        private const string PhModuleName = "{{ .ModuleName }}";
        private const string PhRoadmapUrl = "{{ .RoadmapUrl }}";

        public Task<string> BuildConfirmEmailAsync(string confirmationUrl, string email) =>
            BuildFromTemplateAsync(ConfirmTemplate, new Dictionary<string, string?>
            {
                [PhConfirmationUrl] = confirmationUrl,
                [PhEmail] = email
            });

        public Task<string> BuildResetPasswordEmailAsync(string resetUrl, string email) =>
            BuildFromTemplateAsync(ResetPasswordTemplate, new Dictionary<string, string?>
            {
                [PhConfirmationUrl] = resetUrl,
                [PhEmail] = email
            });

        public Task<string> BuildWelcomeToCourseEmailAsync(string studentName, string courseName, string courseUrl, string email) =>
            BuildFromTemplateAsync(WelcomeCourseTemplate, new Dictionary<string, string?>
            {
                [PhStudentName] = studentName,
                [PhCourseName] = courseName,
                [PhCourseUrl] = courseUrl,
                [PhEmail] = email
            });

        public Task<string> BuildCourseCompletedEmailAsync(string studentName, string courseName, string certificateUrl, string email) =>
            BuildFromTemplateAsync(CourseCompletedTemplate, new Dictionary<string, string?>
            {
                [PhStudentName] = studentName,
                [PhCourseName] = courseName,
                [PhCertificateUrl] = certificateUrl,
                [PhEmail] = email
            });

        public Task<string> BuildSendOtpEmailAsync(string otpCode, string email) =>
            BuildFromTemplateAsync(OtpTemplate, new Dictionary<string, string?>
            {
                [PhOtpCode] = otpCode,
                [PhEmail] = email
            });

        public Task<string> BuildPlanReadyEmailAsync(string studentName, string planName, string roadmapName, string planUrl, string email) =>
            BuildFromTemplateAsync(PlanReadyTemplate, new Dictionary<string, string?>
            {
                [PhStudentName] = studentName,
                [PhPlanName] = planName,
                [PhRoadmapName] = roadmapName,
                [PhPlanUrl] = planUrl,
                [PhEmail] = email
            });

        public Task<string> BuildPremiumUpgradeEmailAsync(string studentName, string packageName, string invoiceNumber, string invoiceDate, string invoiceUrl, string email) =>
            BuildFromTemplateAsync(PremiumUpgradeTemplate, new Dictionary<string, string?>
            {
                [PhStudentName] = studentName,
                [PhPackageName] = packageName,
                [PhInvoiceNumber] = invoiceNumber,
                [PhInvoiceDate] = invoiceDate,
                [PhInvoiceUrl] = invoiceUrl,
                [PhEmail] = email
            });

        public Task<string> BuildModuleCompletedEmailAsync(string studentName, string moduleName, string roadmapName, string roadmapUrl, string email) =>
            BuildFromTemplateAsync(ModuleCompletedTemplate, new Dictionary<string, string?>
            {
                [PhStudentName] = studentName,
                [PhModuleName] = moduleName,
                [PhRoadmapName] = roadmapName,
                [PhRoadmapUrl] = roadmapUrl,
                [PhEmail] = email
            });

        private static async Task<string> BuildFromTemplateAsync(string templateFileName, IReadOnlyDictionary<string, string?> tokens)
        {
            var html = await LoadTemplateAsync(templateFileName);

            foreach (var token in tokens)
            {
                html = html.Replace(token.Key, token.Value ?? string.Empty);
            }

            return html;
        }

        private static async Task<string> LoadTemplateAsync(string templateFileName)
        {
            var asm = Assembly.GetExecutingAssembly();
            var resourcePath = $"{typeof(MailTemplateBuilder).Namespace}.Templates.{templateFileName}";

            await using var stream = asm.GetManifestResourceStream(resourcePath)
                ?? throw new FileNotFoundException($"Email template not found: {resourcePath}");

            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
