using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class Survey
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string Code { get; set; } = null!;
    public string Status { get; set; } = null!;

    // Navigation
    public ICollection<SurveyQuestion> Questions { get; set; } = new HashSet<SurveyQuestion>();
    public ICollection<SurveyResponse> Responses { get; set; } = new HashSet<SurveyResponse>();
}