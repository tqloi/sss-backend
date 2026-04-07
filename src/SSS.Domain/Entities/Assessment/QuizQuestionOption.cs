namespace SSS.Domain.Entities.Assessment;

public class QuizQuestionOption
{
    public long Id { get; set; }
    public long QuestionId { get; set; }
    public string ValueKey { get; set; } = null!;
    public string DisplayText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public decimal? ScoreValue { get; set; }
    public int OrderNo { get; set; }

    // Navigation
    public virtual QuizQuestion Question { get; set; } = null!;
    public virtual ICollection<QuizAnswer> Answers { get; set; } = new HashSet<QuizAnswer>();
}