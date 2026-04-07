using SSS.Application.Features.UserGamifications.Common;

namespace SSS.Application.Abstractions.Services;

public interface IUserGamificationRealtimeService
{
    Task NotifyGamificationUpdatedAsync(string userId, UserGamificationDto data, CancellationToken ct = default);
}
