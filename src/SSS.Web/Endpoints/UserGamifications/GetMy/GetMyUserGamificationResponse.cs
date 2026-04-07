namespace SSS.WebApi.Endpoints.UserGamifications.GetMy;

public sealed class GetMyUserGamificationResponse
{
    public long Id { get; set; }
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public int? TotalExp { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
