using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using SSS.Domain.Entities.Identity;
using SSS.Application.Abstractions.External.Communication.Email;

namespace SSS.WebApi.Endpoints.Auth.ResetPassword
{
    public sealed class ForgotPasswordEndpoint(
        UserManager<User> userManager,
        IMailTemplateBuilder mailTpl,
        IConfiguration config,
        ISmtpEmailSender mail
        ) : Endpoint<ForgotPasswordRequest>
    {
        public override void Configure()
        {
            Post("/api/auth/forgot-password");
            AllowAnonymous();
            Description(d => d.WithTags("Auth"));
        }

        public override async Task HandleAsync(ForgotPasswordRequest req, CancellationToken ct)
        {
            var user = await userManager.FindByEmailAsync(req.Email);
            if (user == null || !await userManager.IsEmailConfirmedAsync(user))
            {
                await SendOkAsync(new { message = "If your email is registered, you will receive a password reset email." }, ct);
                return;
            }

            //var returnUrl = HttpContext.Request.Query["returnUrl"].FirstOrDefault() ?? "/";
            var rawToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(rawToken));

            var feBaseUrl = (config["Frontend:BaseUrl"] ?? string.Empty).TrimEnd('/');
            var feResetPath = (config["Frontend:ResetPath"] ?? "/reset-password").Trim();

            // chuẩn hoá
            feBaseUrl = feBaseUrl.TrimEnd('/');
            feResetPath = feResetPath.StartsWith("/") ? feResetPath : "/" + feResetPath;

            var resetUrl = $"{feBaseUrl}{feResetPath}"
                           + $"?userId={user.Id}"
                           + $"&token={encodedToken}";
            //+ (string.IsNullOrEmpty(returnUrl) ? "" : $"&returnUrl={Uri.EscapeDataString(returnUrl)}");

            var emailBody = await mailTpl.BuildResetPasswordEmailAsync(
                      resetUrl: resetUrl!,
                      email: user.Email
                  );

            // 4. Gửi mail
            await mail.SendMailAsync(new EmailContent
            {
                To = user.Email,
                Subject = "Reset your SSS password",
                Body = emailBody
            });

            await SendOkAsync(new { message = "If your email is registered, you will receive a password reset email." }, ct);
        }
    }
}
