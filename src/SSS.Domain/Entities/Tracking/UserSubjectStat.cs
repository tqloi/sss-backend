using SSS.Domain.Entities.Identity;
using SSS.Domain.Entities.Content;

namespace SSS.Domain.Entities.Tracking;

public class UserSubjectStat
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public long SubjectId { get; set; }
    public decimal? ProficiencyLevel { get; set; }
    public decimal? TotalHoursSpent { get; set; }
    public string? WeakNodeIds { get; set; }
    public DateTime? UpdatedAt { get; set; }

    // Navigation
    public User User { get; set; } = null!;
    public LearningSubject Subject { get; set; } = null!;
}   