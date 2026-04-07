namespace SSS.WebApi.Endpoints.UserGamifications.GetAll;

public sealed class GetAllUserGamificationsResponse
{
    public List<UserGamificationItem> Items { get; set; } = new();

    public class UserGamificationItem
    {
        public long Id { get; set; }
        public string UserId { get; set; } = default!;
        public int? CurrentStreak { get; set; }
        public int? LongestStreak { get; set; }
        public DateTime? LastActiveDate { get; set; }
        public int? TotalExp { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
