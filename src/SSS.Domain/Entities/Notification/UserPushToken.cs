using SSS.Domain.Entities.Identity;

namespace SSS.Domain.Entities.Notification;

public class UserPushToken
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public string DeviceToken { get; set; } = null!;
    public string DeviceType { get; set; } = null!;
    public bool IsActive { get; set; }
    public DateTime LastUpdated { get; set; }

    // Navigation
    public virtual User User { get; set; } = null!;
}