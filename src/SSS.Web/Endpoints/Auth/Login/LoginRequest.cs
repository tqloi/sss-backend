namespace SSS.WebApi.Endpoints.Auth.Login
{
    public sealed class LoginRequest
    {
        public string EmailOrUserName { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string? ReturnUrl { get; set; }
    }
}
