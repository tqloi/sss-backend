using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Assessment;

public class QuizQuestion
{
    public long Id { get; set; }
    public long QuizId { get; set; }
    public string QuestionKey { get; set; } = null!;
    public string Prompt { get; set; } = null!;
    public QuizQuestionType Type { get; set; } = QuizQuestionType.Scale;
    public decimal ScoreWeight { get; set; }
    public int OrderNo { get; set; }
    public bool IsRequired { get; set; }

    // Navigation
    public Quiz Quiz { get; set; } = null!;
    public ICollection<QuizQuestionOption> Options { get; set; } = new HashSet<QuizQuestionOption>();
    public ICollection<QuizAnswer> Answers { get; set; } = new HashSet<QuizAnswer>();
}