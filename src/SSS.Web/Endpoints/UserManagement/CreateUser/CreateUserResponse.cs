namespace SSS.Web.Endpoints.UserManagement.CreateUser
{
    public sealed class CreateUserResponse
    {
        public string Message { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string RoleName { get; set; } = null!;
    }
}
