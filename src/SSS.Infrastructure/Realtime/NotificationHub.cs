using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace SSS.Infrastructure.Realtime;

[Authorize]
public sealed class NotificationHub : Hub
{
}
