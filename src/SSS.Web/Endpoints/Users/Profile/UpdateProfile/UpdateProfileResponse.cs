namespace SSS.WebApi.Endpoints.Users.Profile.UpdateProfile;

public sealed class UpdateProfileResponse
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? UserName { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}
