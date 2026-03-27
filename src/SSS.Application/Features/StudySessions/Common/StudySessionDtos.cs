namespace SSS.Application.Features.StudySessions.Common
{
    // ─── Start Session ───
    public class StartSessionResponse
    {
        public string SessionId { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public string Status { get; set; } = null!;
        public SessionNodeDto? Node { get; set; }
        public IEnumerable<SessionTaskDto> Tasks { get; set; } = [];
    }

    // ─── Pause / Resume ───
    public class PauseSessionResponse
    {
        public string SessionId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public int PauseCount { get; set; }
        public int PauseSeconds { get; set; }
    }

    public class ResumeSessionResponse
    {
        public string SessionId { get; set; } = null!;
        public string Status { get; set; } = null!;
    }

    // ─── End Session (Summary) ───
    public class SessionSummaryResponse
    {
        public string SessionId { get; set; } = null!;
        public int TotalDurationSeconds { get; set; }
        public int TasksCompleted { get; set; }
        public int TotalTasks { get; set; }
        public int XpEarned { get; set; }
    }

    // ─── Session Detail ───
    public class SessionDetailDto
    {
        public string Id { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public long? StudyPlanId { get; set; }
        public long? NodeId { get; set; }
        public long? ModuleId { get; set; }
        public DateTime StartAt { get; set; }
        public DateTime? EndAt { get; set; }
        public string Status { get; set; } = null!;
        public string? EndedReason { get; set; }
        public int? PlannedDurationSeconds { get; set; }
        public int? ActualDurationSeconds { get; set; }
        public int PauseCount { get; set; }
        public int PauseSeconds { get; set; }
        public int? SelfRating { get; set; }
        public string? Timezone { get; set; }
        public DateTime? CreatedAt { get; set; }
        public SessionNodeDto? Node { get; set; }
        public SessionPlanDto? Plan { get; set; }
    }

    // ─── Active Session ───
    public class ActiveSessionDto
    {
        public string SessionId { get; set; } = null!;
        public string Status { get; set; } = null!;
        public DateTime StartAt { get; set; }
        public int ElapsedSeconds { get; set; }
        public long? PlanId { get; set; }
        public long? NodeId { get; set; }
        public string? NodeTitle { get; set; }
        public string? PlanTitle { get; set; }
        public IEnumerable<SessionTaskDto> Tasks { get; set; } = [];
    }

    // ─── Session History Item ───
    public class SessionHistoryItemDto
    {
        public string Id { get; set; } = null!;
        public string Date { get; set; } = null!;
        public string? NodeTitle { get; set; }
        public string? PlanTitle { get; set; }
        public int DurationSeconds { get; set; }
        public int TasksCompleted { get; set; }
        public int TotalTasks { get; set; }
        public int XpEarned { get; set; }
        public int? Rating { get; set; }
        public string Status { get; set; } = null!;
    }

    // ─── Session Statistics ───
    public class SessionStatisticsDto
    {
        public int TotalSessions { get; set; }
        public int TotalSeconds { get; set; }
        public int AverageSessionLengthSeconds { get; set; }
        public double CompletionRate { get; set; }
        public int CurrentStreak { get; set; }
        public int LongestStreak { get; set; }
        public int SessionsThisWeek { get; set; }
        public int SecondsThisWeek { get; set; }
        public int TotalXpEarned { get; set; }
        public double AverageRating { get; set; }
    }

    // ─── Recent Session (Dashboard) ───
    public class RecentSessionDto
    {
        public string Id { get; set; } = null!;
        public int DurationSeconds { get; set; }
        public int TasksCompleted { get; set; }
        public int TotalTasks { get; set; }
        public string Date { get; set; } = null!;
        public int? Rating { get; set; }
        public string? NodeTitle { get; set; }
    }

    // ─── Sub DTOs ───
    public class SessionNodeDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }

    public class SessionPlanDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
    }

    public class SessionTaskDto
    {
        public long Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int Order { get; set; }
        public int? EstimatedMinutes { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class EndSessionTaskDto
    {
        public long TaskId { get; set; }
        public DateTime? EndTime { get; set; }
    }

    // ─── Study Event ───
    public class StudyEventDto
    {
        public string Id { get; set; } = null!;
        public string SessionId { get; set; } = null!;
        public string EventType { get; set; } = null!;
        public long? TaskId { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
