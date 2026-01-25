using MediatR;
using SSS.Application.Features.UserProfile.Common;

namespace SSS.Application.Features.UserProfile.GetProfile;

public sealed class GetProfileQuery : IRequest<UserProfileDto>
{
    public string UserId { get; set; } = default!;
}
