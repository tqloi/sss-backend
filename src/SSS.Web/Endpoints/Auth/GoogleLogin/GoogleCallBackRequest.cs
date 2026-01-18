namespace SSS.WebApi.Endpoints.Auth.GoogleLogin
{
    public sealed class GoogleCallbackRequest
    {
        public string? ReturnUrl { get; init; } = "/";
        public string? Opener { get; init; } 
    }
}
