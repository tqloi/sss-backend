using MediatR;

namespace SSS.Application.Features.Notification.MarkAllAsRead;

public sealed class MarkAllNotificationsAsReadCommand : IRequest<MarkAllNotificationsAsReadResult>
{
    public string UserId { get; set; } = default!;
}
