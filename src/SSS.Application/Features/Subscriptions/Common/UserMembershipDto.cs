namespace SSS.Application.Features.Subscriptions.Common;

public sealed class UserMembershipDto
{
    public string UserId { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string? SubscriptionType { get; set; }
    public DateTime? SubscriptionStartDate { get; set; }
    public DateTime? SubscriptionEndDate { get; set; }
    public bool HasActiveSubscription { get; set; }
    public int DaysRemaining { get; set; }
}
