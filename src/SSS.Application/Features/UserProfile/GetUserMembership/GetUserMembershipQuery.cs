using MediatR;
using SSS.Application.Features.UserProfile.Common;

namespace SSS.Application.Features.UserProfile.GetUserMembership;

public sealed class GetUserMembershipQuery : IRequest<UserMembershipDto>
{
    public string UserId { get; set; } = default!;
}
