using SSS.Application.Features.Auth.Common;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using SSS.Infrastructure.External.Identity.Google;
using SSS.Application.Abstractions.Sercurity.Jwt;
using SSS.Domain.Entities.Identity;

namespace SSS.Application.Features.Auth.GoogleCallBack
{
    public sealed class GoogleCallbackHandler(
        UserManager<User> userManager,
        IGoogleAuthService googleService,
        IJwtTokenService jwtService
    ) : IRequestHandler<GoogleCallbackCommand, AuthResult>
    {
        public async Task<AuthResult> Handle(GoogleCallbackCommand req, CancellationToken ct)
        {
            var info = await googleService.GetExternalLoginInfoAsync();
            if (info is null)
               throw new InvalidOperationException("Failed to get external login info from Google.");

            // 1) tìm user theo email (Google luôn có email)
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrWhiteSpace(email))
                throw new InvalidOperationException("No email from Google.");

            var user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = await googleService.AutoProvisionUserAsync(info);
                await userManager.AddToRoleAsync(user, "User");
            }

            // 2) đảm bảo liên kết external
            var existing = await googleService.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey);
            if (existing is null)
            {
                var add = await googleService.AddLoginAsync(user, info);
                if (!add.Succeeded)
                    throw new InvalidOperationException(add.Errors.First().Description);
            }

            return await jwtService.IssueAsync(user, req.RequestIp, ct);
        }
    }
}