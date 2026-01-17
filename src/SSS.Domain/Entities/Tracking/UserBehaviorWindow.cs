using SSS.Domain.Entities.Identity;

namespace SSS.Domain.Entities.Tracking;

public class UserBehaviorWindow
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime WindowStart { get; set; }
    public DateTime WindowEnd { get; set; }
    public decimal? AvgFocusScore { get; set; }
    public decimal? ActiveRatio { get; set; }
    public int? AvgSessionLengthMinutes { get; set; }
    public decimal? CompletionRate { get; set; }
    public DateTime ComputedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}