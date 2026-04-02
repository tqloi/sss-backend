using MediatR;

namespace SSS.Application.Features.UserGamifications.RecordDailyLogin
{
    public class RecordDailyLoginCommand : IRequest<RecordDailyLoginResult>
    {
        public string UserId { get; set; } = null!;
    }

    public class RecordDailyLoginResult
    {
        public bool Success { get; set; }
        public string Message { get; set; } = null!;
        public UserGamificationDto Data { get; set; } = null!;
    }

    public class UserGamificationDto
    {
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int TotalExp { get; set; }
        public bool StreakUpdatedToday { get; set; }
    }
}
