using System;
using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class Quiz
{
    public long Id { get; set; }
    public long StudyPlanModuleId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public decimal? TotalScore { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public StudyPlanModule StudyPlanModule { get; set; } = null!;
    public ICollection<QuizQuestion> Questions { get; set; } = new HashSet<QuizQuestion>();
    public ICollection<QuizAttempt> Attempts { get; set; } = new HashSet<QuizAttempt>();
}