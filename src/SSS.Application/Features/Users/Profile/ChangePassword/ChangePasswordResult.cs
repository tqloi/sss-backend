namespace SSS.Application.Features.Users.Profile.ChangePassword;

public sealed class ChangePasswordResult
{
    public bool Success { get; set; }
    public string Message { get; set; } = default!;
}
