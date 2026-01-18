using SSS.Application.Features.Auth.Common;

namespace SSS.WebApi.Endpoints.Auth.Login
{
    public sealed class LoginResponse
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiresUtc { get; set; }
        public UserDto User { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
