using System;
using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class TaskItem
{
    public long Id { get; set; }
    public long StudyPlanModuleId { get; set; }
    public string Title { get; set; } = null!;
    public string? Status { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedAt { get; set; }

    // Navigation
    public StudyPlanModule StudyPlanModule { get; set; } = null!;
    public ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}