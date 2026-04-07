namespace SSS.WebApi.Endpoints.Users.GetProfile;

public sealed class GetProfileResponse
{
    public string Id { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public string? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}
