using System.Collections.Generic;

namespace SSS.Domain.Entities;

public class StudyPlanModule
{
    public long Id { get; set; }
    public long StudyPlanId { get; set; }
    public long RoadmapNodeId { get; set; }
    public string? Status { get; set; }

    // Navigation
    public StudyPlan StudyPlan { get; set; } = null!;
    public RoadmapNode RoadmapNode { get; set; } = null!;
    public ICollection<TaskItem> Tasks { get; set; } = new HashSet<TaskItem>();
    public ICollection<Quiz> Quizzes { get; set; } = new HashSet<Quiz>();
    public ICollection<StudySession> StudySessions { get; set; } = new HashSet<StudySession>();
}