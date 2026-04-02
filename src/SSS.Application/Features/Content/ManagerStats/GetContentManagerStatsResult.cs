namespace SSS.Application.Features.Content.ManagerStats
{
    public sealed record GetContentManagerStatsResult(ContentManagerStatsDto Stats);

    public sealed class ContentManagerStatsDto
    {
        public long TotalRoadmapsCreated { get; set; }
        public long TotalNodesAdded { get; set; }
        public long TotalNodeContentsCreated { get; set; }
        public long TotalQuizzesCreated { get; set; }

        public long TotalUsersCompletedRoadmaps { get; set; }
        public long TotalUsersInProgressRoadmaps { get; set; }

        public TopRoadmapStatsDto? TopRoadmapMostLearned { get; set; }
        public List<MonthlyCompletedUsersDto> CompletedUsersByMonth { get; set; } = new();

        public QuizLeaderboardItemDto? MostAttemptedQuiz { get; set; }
        public QuizLeaderboardItemDto? MostPassedQuiz { get; set; }
        public QuizLeaderboardItemDto? MostFailedQuiz { get; set; }
    }

    public sealed class TopRoadmapStatsDto
    {
        public long RoadmapId { get; set; }
        public string Title { get; set; } = null!;
        public long SubjectId { get; set; }
        public long StudyPlanCount { get; set; }
        public long NodeCount { get; set; }
    }

    public sealed class MonthlyCompletedUsersDto
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public long CompletedUsers { get; set; }
    }

    public sealed class QuizLeaderboardItemDto
    {
        public long QuizId { get; set; }
        public string? QuizTitle { get; set; }
        public long RoadmapId { get; set; }
        public string RoadmapTitle { get; set; } = null!;
        public long Count { get; set; }
    }
}
