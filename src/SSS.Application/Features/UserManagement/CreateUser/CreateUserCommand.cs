using MediatR;

namespace SSS.Application.Features.UserManagement.CreateUser
{
    public sealed class CreateUserCommand : IRequest<CreateUserResult>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string ConfirmPassword { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string? LastName { get; set; }
        public string RoleName { get; set; } = null!;
        public string? CreatedBy { get; set; }
    }
}
