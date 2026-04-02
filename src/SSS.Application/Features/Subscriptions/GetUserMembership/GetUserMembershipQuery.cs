using MediatR;
using SSS.Application.Features.Subscriptions.Common;

namespace SSS.Application.Features.Subscriptions.GetUserMembership;

public sealed class GetUserMembershipQuery : IRequest<UserMembershipDto>
{
    public string UserId { get; set; } = default!;
}
