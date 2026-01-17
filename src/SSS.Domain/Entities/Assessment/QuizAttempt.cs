using SSS.Domain.Entities.Identity;
using SSS.Domain.Enums;

namespace SSS.Domain.Entities.Assessment;

public class QuizAttempt
{
    public long Id { get; set; }
    public long QuizId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public QuizAttemptStatus Status { get; set; } = QuizAttemptStatus.InProgress;

    // Navigation
    public Quiz Quiz { get; set; } = null!;
    public User User { get; set; } = null!;
    public ICollection<QuizAnswer> Answers { get; set; } = new HashSet<QuizAnswer>();
}