using MediatR;

namespace SSS.Application.Features.Users.Profile.GetProfile;

public sealed class GetProfileQuery : IRequest<ProfileResult>
{
    public string UserId { get; set; } = default!;
}
