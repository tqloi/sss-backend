namespace SSS.Application.Features.Auth.Register
{
    public sealed class RegisterResult
    {
        public string UserId { get; set; }
        public string Email { get; set; }
        public string EncodedEmailConfirmToken { get; set; }
    }
}
