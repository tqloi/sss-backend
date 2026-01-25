using MediatR;
using SSS.Application.Features.UserGamifications.Common;

namespace SSS.Application.Features.UserGamifications.GetUserGamification;

public sealed class GetUserGamificationQuery : IRequest<UserGamificationDto?>
{
    public string UserId { get; set; } = default!;
}
