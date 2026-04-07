using MediatR;

namespace SSS.Application.Features.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadCommand : IRequest<MarkNotificationAsReadResult>
{
    public string UserId { get; set; } = default!;
    public long NotificationId { get; set; }
}
