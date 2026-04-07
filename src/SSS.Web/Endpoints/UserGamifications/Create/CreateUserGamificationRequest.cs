namespace SSS.WebApi.Endpoints.UserGamifications.Create;

public sealed class CreateUserGamificationRequest
{
    public int? CurrentStreak { get; set; }
    public int? LongestStreak { get; set; }
    public DateTime? LastActiveDate { get; set; }
    public int? TotalExp { get; set; }
}
