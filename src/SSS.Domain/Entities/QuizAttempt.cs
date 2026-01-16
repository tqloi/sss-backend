using System;
using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class QuizAttempt
{
    public long Id { get; set; }
    public long QuizId { get; set; }
    public string UserId { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public decimal? Score { get; set; }
    public string Status { get; set; } = null!;

    // Navigation
    public Quiz Quiz { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<QuizAnswer> Answers { get; set; } = new HashSet<QuizAnswer>();
}