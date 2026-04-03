using Microsoft.AspNetCore.SignalR;
using SSS.Application.Abstractions.Services;
using SSS.Application.Features.UserGamifications.Common;
using SSS.Infrastructure.Realtime;

namespace SSS.Infrastructure.Services;

public class UserGamificationRealtimeService(IHubContext<UserGamificationHub> hubContext) : IUserGamificationRealtimeService
{
    public Task NotifyGamificationUpdatedAsync(string userId, UserGamificationDto data, CancellationToken ct = default)
    {
        return hubContext.Clients.User(userId)
            .SendAsync("gamification.updated", data, ct);
    }
}
