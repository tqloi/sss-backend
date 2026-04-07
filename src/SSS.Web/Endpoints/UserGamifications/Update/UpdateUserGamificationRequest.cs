namespace SSS.WebApi.Endpoints.UserGamifications.Update;

public sealed class UpdateUserGamificationRequest
{
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public int? TotalExp { get; set; }
}
