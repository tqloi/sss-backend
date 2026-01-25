using Microsoft.AspNetCore.Identity;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Identity;

public class User : IdentityUser
{
    public string FirstName { get; set; }
    public string? LastName { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Address { get; set; }
    public DateTime? Dob { get; set; }
    public Gender? Gender { get; set; }
    public SubscriptionType? SubscriptionType { get; set; } = Enums.SubscriptionType.Free;
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public bool? IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new HashSet<RefreshToken>();
    // Add other navigation collections as needed (e.g., StudyPlans, UserNotifications, etc.)
}