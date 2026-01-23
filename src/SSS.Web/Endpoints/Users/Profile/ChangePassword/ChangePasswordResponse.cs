namespace SSS.WebApi.Endpoints.Users.Profile.ChangePassword;

public sealed class ChangePasswordResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = default!;
}
