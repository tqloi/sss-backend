namespace SSS.Application.Features.UserManagement.CreateUser
{
    public sealed class CreateUserResult
    {
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}
