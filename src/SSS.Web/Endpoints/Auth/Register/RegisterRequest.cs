namespace SSS.WebApi.Endpoints.Auth.Register
{
    public sealed class RegisterRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { set; get; }
        public string? LastName { set; get; }
        public string? ReturnUrl { set; get; }
    }
}
