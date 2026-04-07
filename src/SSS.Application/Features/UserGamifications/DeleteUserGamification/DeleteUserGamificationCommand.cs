using MediatR;

namespace SSS.Application.Features.UserGamifications.DeleteUserGamification;

public sealed class DeleteUserGamificationCommand : IRequest<bool>
{
    public string UserId { get; set; } = default!;
}
