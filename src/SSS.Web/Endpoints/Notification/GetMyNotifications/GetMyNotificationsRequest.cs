namespace SSS.WebApi.Endpoints.Notification.GetMyNotifications;

public sealed class GetMyNotificationsRequest
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
