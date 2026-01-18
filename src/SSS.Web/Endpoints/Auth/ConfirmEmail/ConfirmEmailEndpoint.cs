using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using SSS.Domain.Entities.Identity;
using System.Text;

namespace SSS.WebApi.Endpoints.Auth.ConfirmEmail
{
    public sealed class ConfirmEmailEndpoint(
        UserManager<User> userManager)
      : Endpoint<ConfirmEmailRequest>
    {
        public const string Route = "/api/auth/confirm-email";
        public override void Configure()
        {
            Get(Route);
            AllowAnonymous();
            Description(d => d.WithTags("Auth"));
        }

        public override async Task HandleAsync(ConfirmEmailRequest req, CancellationToken ct)
        {
            // 1) Tìm user
            var user = await userManager.FindByIdAsync(req.UserId);
            if (user is null)
            {
                await SendAsync(new { message = "User does not exist" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            // 2) Giải mã token (Base64Url)
            string rawToken;
            try
            {
                var bytes = WebEncoders.Base64UrlDecode(req.Token);
                rawToken = Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                await SendAsync(new { message = "Invalid token" }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            // 3) Confirm email
            var result = await userManager.ConfirmEmailAsync(user, rawToken);
            if (!result.Succeeded)
            {
                await SendAsync(new
                {
                    message = "Otp confirmation failed",
                    errors = result.Errors.Select(e => e.Description)
                }, StatusCodes.Status400BadRequest, ct);
                return;
            }

            // 4) Redirect nếu có returnUrl hợp lệ, ngược lại trả JSON
            if (IsLocalUrl(req.ReturnUrl))
            {
                HttpContext.Response.Redirect(req.ReturnUrl!);
                return;
            }

            await SendOkAsync(new { message = "Otp confirmation successful" }, ct);
        }

        // Minimal local-URL check để tránh open redirect
        static bool IsLocalUrl(string? url)
            => !string.IsNullOrWhiteSpace(url)
               && url.StartsWith('/')
               && !url.StartsWith("//")
               && !url.StartsWith(@"/\");
    }
}