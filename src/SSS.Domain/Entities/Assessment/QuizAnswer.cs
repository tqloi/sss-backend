namespace SSS.Domain.Entities.Assessment;

public class QuizAnswer
{
    public long Id { get; set; }
    public long AttemptId { get; set; }
    public long QuestionId { get; set; }
    public long? OptionId { get; set; }
    public string? TextValue { get; set; }
    public decimal? NumberValue { get; set; }
    public decimal? ScoredValue { get; set; }
    public DateTime AnsweredAt { get; set; }

    // Navigation
    public virtual QuizAttempt Attempt { get; set; } = null!;
    public virtual QuizQuestion Question { get; set; } = null!;
    public virtual QuizQuestionOption? Option { get; set; }
}