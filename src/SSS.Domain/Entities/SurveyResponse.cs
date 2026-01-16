using System;
using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class SurveyResponse
{
    public long Id { get; set; }
    public long SurveyId { get; set; }
    public string UserId { get; set; } = null!;
    public string TriggerReason { get; set; } = null!;
    public DateTime StartedAt { get; set; }
    public DateTime? SubmittedAt { get; set; }
    public string? SnapshotJson { get; set; }

    // Navigation
    public Survey Survey { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
    public ICollection<SurveyAnswer> Answers { get; set; } = new HashSet<SurveyAnswer>();
}