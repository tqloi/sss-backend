using SSS.Domain.Enums;

namespace SSS.WebApi.Endpoints.Users.Profile.UpdateProfile;

public sealed class UpdateProfileRequest
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public Gender? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}
