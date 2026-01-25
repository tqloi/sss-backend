using SSS.Domain.Entities.Content;

namespace SSS.Domain.Entities.Assessment;

public class Quiz
{
    public long Id { get; set; }
    public long RoadmapNodeId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? TotalScore { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public virtual RoadmapNode RoadmapNode { get; set; } = null!;
    public virtual ICollection<QuizQuestion> Questions { get; set; } = new HashSet<QuizQuestion>();
    public virtual ICollection<QuizAttempt> Attempts { get; set; } = new HashSet<QuizAttempt>();
}