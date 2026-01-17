namespace SSS.Domain.Entities.Assessment;

public class SurveyQuestionOption
{
    public long Id { get; set; }
    public long QuestionId { get; set; }
    public string ValueKey { get; set; } = null!;
    public string DisplayText { get; set; } = null!;
    public decimal? Weight { get; set; }
    public int OrderNo { get; set; }
    public bool AllowFreeText { get; set; }

    // Navigation
    public SurveyQuestion Question { get; set; } = null!;
    public ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
}