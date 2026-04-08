namespace SSS.Application.Features.AdminAnalytics.GetOverview
{
    public sealed record GetAdminDashboardOverviewResult(AdminDashboardOverviewDto Data);

    public sealed class AdminDashboardOverviewDto
    {
        public AdminDashboardSummaryDto Summary { get; set; } = new();
        public List<RoleDistributionItemDto> RoleDistribution { get; set; } = new();
        public LearningCoverageDto LearningCoverage { get; set; } = new();
        public RoadmapStatusBreakdownDto RoadmapStatusBreakdown { get; set; } = new();
    }

    public sealed class AdminDashboardSummaryDto
    {
        public long TotalUsers { get; set; }
        public long ActiveUsers { get; set; }
        public long InactiveUsers { get; set; }

        public long ContentManagers { get; set; }
        public long AssignedContentManagers { get; set; }
        public long Analysts { get; set; }

        public long TotalLatestRoadmaps { get; set; }
        public long ActiveLatestRoadmaps { get; set; }
    }

    public sealed class RoleDistributionItemDto
    {
        public string Role { get; set; } = null!;
        public long Count { get; set; }
        public int Percentage { get; set; }
    }

    public sealed class LearningCoverageDto
    {
        public long ActiveCategories { get; set; }
        public long TotalCategories { get; set; }
        public long ActiveSubjects { get; set; }
        public long TotalSubjects { get; set; }
        public int CategoryActivationRate { get; set; }
        public int SubjectActivationRate { get; set; }
        public double AverageSubjectsPerCategory { get; set; }
    }

    public sealed class RoadmapStatusBreakdownDto
    {
        public long Active { get; set; }
        public long Draft { get; set; }
        public long Archived { get; set; }
        public long Unknown { get; set; }
        public long Total { get; set; }
        public int ActiveRate { get; set; }
    }
}
