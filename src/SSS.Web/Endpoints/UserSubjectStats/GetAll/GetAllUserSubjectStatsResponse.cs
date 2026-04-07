namespace SSS.WebApi.Endpoints.UserSubjectStats.GetAll;

public sealed class GetAllUserSubjectStatsResponse
{
    public List<UserSubjectStatItem> Items { get; set; } = new();

    public class UserSubjectStatItem
    {
        public long Id { get; set; }
        public string UserId { get; set; } = default!;
        public long SubjectId { get; set; }
        public string? SubjectName { get; set; }
        public decimal? ProficiencyLevel { get; set; }
        public decimal? TotalHoursSpent { get; set; }
        public string? WeakNodeIds { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
