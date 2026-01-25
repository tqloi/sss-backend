namespace SSS.WebApi.Endpoints.Users.UpdateProfile;

public sealed class UpdateProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}
