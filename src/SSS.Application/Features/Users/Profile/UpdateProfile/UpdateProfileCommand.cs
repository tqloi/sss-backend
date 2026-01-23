using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Users.Profile.UpdateProfile;

public sealed class UpdateProfileCommand : IRequest<ProfileResult>
{
    public string UserId { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public Gender? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}
