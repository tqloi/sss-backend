using MediatR;

namespace SSS.Application.Features.Notification.GetMyNotifications;

public sealed class GetMyNotificationsQuery : IRequest<GetMyNotificationsResult>
{
    public string UserId { get; set; } = default!;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
