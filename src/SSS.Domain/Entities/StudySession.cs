using System;

namespace SSS.Domain.Entities;

public class StudySession
{
    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public long? TaskId { get; set; }
    public long? StudyPlanId { get; set; }
    public long? ModuleId { get; set; }
    public long? NodeId { get; set; }
    public DateTime StartAt { get; set; }
    public DateTime? EndAt { get; set; }
    public string? EndedReason { get; set; }
    public int? PlannedDurationSeconds { get; set; }
    public int? ActualDurationSeconds { get; set; }
    public int? ActiveSeconds { get; set; }
    public int? IdleSeconds { get; set; }
    public int? PauseCount { get; set; }
    public int? PauseSeconds { get; set; }
    public int? FocusScore { get; set; }
    public decimal? ConfidenceActiveLearning { get; set; }
    public int? FatigueScore { get; set; }
    public int? SelfRating { get; set; }
    public string? LocalTimeBlock { get; set; }
    public string? Timezone { get; set; }
    public DateTime? CreatedAt { get; set; }

    // Navigation
    public ApplicationUser User { get; set; } = null!;
    public TaskItem? Task { get; set; }
    public StudyPlan? StudyPlan { get; set; }
    public StudyPlanModule? Module { get; set; }
    public RoadmapNode? Node { get; set; }
}