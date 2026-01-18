namespace SSS.WebApi.Endpoints.Auth.ConfirmEmail
{
    public sealed class ConfirmEmailRequest
    {
        public string UserId { get; init; } = default!;
        public string Token { get; init; } = default!;
        public string? ReturnUrl { get; init; }
    }
}