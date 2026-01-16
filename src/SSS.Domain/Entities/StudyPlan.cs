using System;
using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class StudyPlan
{
    public long Id { get; set; }
    public string UserId { get; set; } = null!;
    public long RoadmapId { get; set; }
    public int? ProfileVersion { get; set; }
    public string? Status { get; set; }
    public string? Strategy { get; set; }
    public DateTime CreatedAt { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public Roadmap Roadmap { get; set; } = null!;
    public ICollection<StudyPlanModule> Modules { get; set; } = new HashSet<StudyPlanModule>();
    public ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}