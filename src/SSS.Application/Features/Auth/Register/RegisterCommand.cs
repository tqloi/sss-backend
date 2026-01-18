using MediatR;

namespace SSS.Application.Features.Auth.Register
{
    public sealed class RegisterCommand : IRequest<RegisterResult>
    {
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FirstName { set; get; }
        public string? LastName { set; get; }
        public string? ReturnUrl { set; get; }
    }
}
