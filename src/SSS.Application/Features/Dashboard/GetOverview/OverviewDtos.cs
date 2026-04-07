using SSS.Application.Features.StudySessions.Common;

namespace SSS.Application.Features.Dashboard.GetOverview
{
    public class OverviewStudyPlanDto
    {
        public long StudyPlanId { get; set; }
        public string RoadmapTitle { get; set; } = null!;
        public double ProgressPercentage { get; set; }
        public int TotalXpEarned { get; set; }
        public int StudyStreakDays { get; set; }
        
        public OverviewTaskDto? TodaysFocus { get; set; }
        public IEnumerable<OverviewTaskDto> UpcomingTasks { get; set; } = [];
        public IEnumerable<RecentSessionDto> RecentSessions { get; set; } = [];
    }

    public class OverviewTaskDto
    {
        public long TaskId { get; set; }
        public long ModuleId { get; set; }
        public string ModuleTitle { get; set; } = null!;
        public string TaskTitle { get; set; } = null!;
        public string? Description { get; set; }
        public int EstimatedMinutes { get; set; }
    }
}
