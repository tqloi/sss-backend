namespace SSS.Application.Features.Auth.Common
{
    public class AuthResult
    {
        public string AccessToken { get; set; } = default!;
        public DateTime AccessTokenExpiresUtc { get; set; }   

        public string RefreshTokenPlain { get; set; } = default!;
        public DateTime RefreshTokenExpiresUtc { get; set; }  

        public UserDto User { get; set; } = default!;
        public string? ReturnUrl { get; set; } = "/";
    }

    public sealed class UserDto
    {
        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? AvatarUrl { get; set; }
        public IReadOnlyList<string> Roles { get; set; } = Array.Empty<string>();
    }
}
