using System;

namespace SSS.Domain.Entities;

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
    public SurveyResponse Response { get; set; } = null!;
    public SurveyQuestion Question { get; set; } = null!;
    public SurveyQuestionOption? Option { get; set; }
}