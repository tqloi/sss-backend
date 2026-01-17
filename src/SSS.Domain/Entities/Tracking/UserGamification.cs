using SSS.Domain.Entities.Identity;

namespace SSS.Domain.Entities.Tracking;

public class UserGamification
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public int? TotalExp { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
}