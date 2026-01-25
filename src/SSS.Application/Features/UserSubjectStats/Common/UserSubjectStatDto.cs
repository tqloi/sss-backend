namespace SSS.Application.Features.UserSubjectStats.Common;

public sealed class UserSubjectStatDto
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
