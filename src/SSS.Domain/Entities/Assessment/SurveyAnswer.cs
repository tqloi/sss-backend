namespace SSS.Domain.Entities.Assessment;

public class SurveyAnswer
{
    public long Id { get; set; }
    public long ResponseId { get; set; }
    public long QuestionId { get; set; }
    public long? OptionId { get; set; }
    public decimal? NumberValue { get; set; }
    public string? TextValue { get; set; }
    public DateTime AnsweredAt { get; set; }

    // Navigation
    public virtual SurveyResponse Response { get; set; } = null!;
    public virtual SurveyQuestion Question { get; set; } = null!;
    public virtual SurveyQuestionOption? Option { get; set; }
}