using System;

namespace SSS.Domain.Entities;

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
    public ApplicationUser User { get; set; } = null!;
}