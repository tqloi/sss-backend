using Microsoft.AspNetCore.Identity;
using SSS.Domain.Entities.Identity;

namespace SSS.Infrastructure.External.Identity.Google
{
    public interface IGoogleAuthService
    {
        Task<ExternalLoginInfo?> GetExternalLoginInfoAsync();
        Task<SignInResult> ExternalLoginSignInAsync(string provider, string providerKey);
        Task<User> AutoProvisionUserAsync(ExternalLoginInfo info);
        Task<IdentityResult> AddLoginAsync(User user, ExternalLoginInfo info);
        Task SignInAsync(User user);
    }
}
