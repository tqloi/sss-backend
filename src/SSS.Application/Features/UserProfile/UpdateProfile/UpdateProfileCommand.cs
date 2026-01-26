using MediatR;
using SSS.Application.Features.UserProfile.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.UserProfile.UpdateProfile;

public sealed class UpdateProfileCommand : IRequest<UserProfileDto>
{
    public string UserId { get; set; } = default!;
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public Gender? Gender { get; set; }
    public string? PhoneNumber { get; set; }
}
