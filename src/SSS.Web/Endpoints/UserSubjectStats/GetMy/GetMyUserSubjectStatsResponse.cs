namespace SSS.WebApi.Endpoints.UserSubjectStats.GetMy;

public sealed class GetMyUserSubjectStatsResponse
{
    public List<UserSubjectStatItem> Items { get; set; } = new();

    public class UserSubjectStatItem
    {
        public long Id { get; set; }
        public long SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal? ProficiencyLevel { get; set; }
        public decimal? TotalHoursSpent { get; set; }
        public string? WeakNodeIds { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
