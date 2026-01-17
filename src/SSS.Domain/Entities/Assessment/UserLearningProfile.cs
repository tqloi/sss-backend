using SSS.Domain.Entities.Identity;

namespace SSS.Domain.Entities.Assessment;

public class UserLearningProfile
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public int ProfileVersion { get; set; }
    public string? TargetRole { get; set; }
    public string? CurrentLevel { get; set; }
    public int? TargetDeadlineMonths { get; set; }
    public string? AvailableDaysJson { get; set; }
    public string? PreferredTimeBlocksJson { get; set; }
    public int? SessionLengthPrefMinutes { get; set; }
    public decimal? WVisual { get; set; }
    public decimal? WReading { get; set; }
    public decimal? WPractice { get; set; }
    public decimal? ConfOverall { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}