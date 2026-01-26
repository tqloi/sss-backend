namespace SSS.WebApi.Endpoints.UserGamifications.Get;

public sealed class GetUserGamificationResponse
{
    public long Id { get; set; }
    public string UserId { get; set; } = default!;
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public int? TotalExp { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
