using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Assessment;

public class Survey
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string Code { get; set; } = null!;
    public SurveyStatus Status { get; set; } = SurveyStatus.Draft;

    // Navigation
    public virtual ICollection<SurveyQuestion> Questions { get; set; } = new HashSet<SurveyQuestion>();
    public virtual ICollection<SurveyResponse> Responses { get; set; } = new HashSet<SurveyResponse>();
}