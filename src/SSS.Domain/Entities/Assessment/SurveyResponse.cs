using SSS.Domain.Entities.Identity;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Assessment;

public class SurveyResponse
{
    public long Id { get; set; }
    public long SurveyId { get; set; }
    public string UserId { get; set; } = null!;
    public SurveyTriggerReason TriggerReason { get; set; } = SurveyTriggerReason.Initial;
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string? SnapshotJson { get; set; }

    // Navigation
    public virtual Survey Survey { get; set; } = null!;
    public virtual User User { get; set; } = null!;
    public virtual ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
}