using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.ManagerStats
{
    public sealed class GetContentManagerStatsHandler(IAppDbContext dbContext)
        : IRequestHandler<GetContentManagerStatsQuery, GetContentManagerStatsResult>
    {
        public async Task<GetContentManagerStatsResult> Handle(GetContentManagerStatsQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ManagerId))
            {
                throw new UnauthorizedAccessException("Manager id is required.");
            }

            var managedSubjectIds = await dbContext.ContentManagerSubjects
                .AsNoTracking()
                .Where(x => x.ContentManagerId == request.ManagerId && x.IsActive)
                .Select(x => x.SubjectId)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (managedSubjectIds.Count == 0)
            {
                return new GetContentManagerStatsResult(new ContentManagerStatsDto());
            }

            if (request.SubjectId.HasValue && !managedSubjectIds.Contains(request.SubjectId.Value))
            {
                return new GetContentManagerStatsResult(new ContentManagerStatsDto());
            }

            var roadmapBaseQuery = dbContext.Roadmaps
                .AsNoTracking()
                .Where(r =>
                    r.CreateById == request.ManagerId
                    && managedSubjectIds.Contains(r.SubjectId));

            if (request.SubjectId.HasValue)
            {
                roadmapBaseQuery = roadmapBaseQuery.Where(r => r.SubjectId == request.SubjectId.Value);
            }

            var roadmaps = await roadmapBaseQuery
                .Select(r => new { r.Id, r.Title, r.SubjectId })
                .ToListAsync(cancellationToken);

            if (roadmaps.Count == 0)
            {
                return new GetContentManagerStatsResult(new ContentManagerStatsDto());
            }

            var roadmapIds = roadmaps.Select(r => r.Id).ToList();

            var totalRoadmapsCreated = roadmaps.Count;

            var nodes = await dbContext.RoadmapNodes
                .AsNoTracking()
                .Where(n => roadmapIds.Contains(n.RoadmapId))
                .Select(n => new { n.Id, n.RoadmapId })
                .ToListAsync(cancellationToken);

            var totalNodesAdded = nodes.Count;
            var nodeIds = nodes.Select(n => n.Id).ToList();

            var totalNodeContentsCreated = nodeIds.Count == 0
                ? 0
                : await dbContext.NodeContents
                    .AsNoTracking()
                    .CountAsync(c => nodeIds.Contains(c.NodeId), cancellationToken);

            var quizzes = nodeIds.Count == 0
                ? new List<QuizRow>()
                : await dbContext.Quizzes
                    .AsNoTracking()
                    .Where(q => nodeIds.Contains(q.RoadmapNodeId))
                    .Join(
                        dbContext.RoadmapNodes.AsNoTracking(),
                        q => q.RoadmapNodeId,
                        n => n.Id,
                        (q, n) => new { q.Id, q.Title, n.RoadmapId })
                    .Select(x => new QuizRow
                    {
                        Id = x.Id,
                        Title = x.Title,
                        RoadmapId = x.RoadmapId
                    })
                    .ToListAsync(cancellationToken);

            var totalQuizzesCreated = quizzes.Count;

            var roadmapStudyPlans = await dbContext.StudyPlans
                .AsNoTracking()
                .Where(sp => roadmapIds.Contains(sp.RoadmapId))
                .Select(sp => new { sp.Id, sp.RoadmapId, sp.UserId })
                .ToListAsync(cancellationToken);

            TopRoadmapStatsDto? topRoadmapMostLearned = null;
            if (roadmapStudyPlans.Count > 0)
            {
                var topRoadmapAgg = roadmapStudyPlans
                    .GroupBy(sp => sp.RoadmapId)
                    .Select(g => new { RoadmapId = g.Key, StudyPlanCount = g.Count() })
                    .OrderByDescending(x => x.StudyPlanCount)
                    .ThenBy(x => x.RoadmapId)
                    .First();

                var roadmapMeta = roadmaps.First(r => r.Id == topRoadmapAgg.RoadmapId);
                var topRoadmapNodeCount = nodes.LongCount(n => n.RoadmapId == topRoadmapAgg.RoadmapId);

                topRoadmapMostLearned = new TopRoadmapStatsDto
                {
                    RoadmapId = roadmapMeta.Id,
                    Title = roadmapMeta.Title,
                    SubjectId = roadmapMeta.SubjectId,
                    StudyPlanCount = topRoadmapAgg.StudyPlanCount,
                    NodeCount = topRoadmapNodeCount
                };
            }

            var studyPlanIds = roadmapStudyPlans.Select(sp => sp.Id).ToList();
            var moduleRows = studyPlanIds.Count == 0
                ? new List<ModuleRow>()
                : await dbContext.StudyPlanModules
                    .AsNoTracking()
                    .Where(m => studyPlanIds.Contains(m.StudyPlanId))
                    .Join(
                        dbContext.StudyPlans.AsNoTracking(),
                        m => m.StudyPlanId,
                        sp => sp.Id,
                        (m, sp) => new { m.StudyPlanId, m.Status, m.CompletedAt, sp.UserId })
                    .Select(x => new ModuleRow
                    {
                        StudyPlanId = x.StudyPlanId,
                        Status = x.Status,
                        CompletedAt = x.CompletedAt,
                        UserId = x.UserId
                    })
                    .ToListAsync(cancellationToken);

            var completedUsers = new HashSet<string>();
            var inProgressUsers = new HashSet<string>();
            var monthlyCompletedSource = new List<(int Year, int Month, string UserId)>();

            if (moduleRows.Count > 0)
            {
                var modulesByPlan = moduleRows
                    .GroupBy(x => x.StudyPlanId)
                    .ToList();

                foreach (var group in modulesByPlan)
                {
                    var list = group.ToList();
                    if (list.Count == 0)
                    {
                        continue;
                    }

                    var userId = list[0].UserId;
                    var isCompleted = list.All(x => x.Status == ModuleStatus.Completed);

                    if (isCompleted)
                    {
                        completedUsers.Add(userId);

                        var completedAt = list
                            .Where(x => x.CompletedAt.HasValue)
                            .Select(x => x.CompletedAt!.Value)
                            .OrderByDescending(x => x)
                            .FirstOrDefault();

                        if (completedAt != default)
                        {
                            monthlyCompletedSource.Add((completedAt.Year, completedAt.Month, userId));
                        }
                    }
                    else
                    {
                        var started = list.Any(x => x.Status == ModuleStatus.Active || x.Status == ModuleStatus.Completed);
                        if (started)
                        {
                            inProgressUsers.Add(userId);
                        }
                    }
                }
            }

            var completedUsersByMonth = monthlyCompletedSource
                .GroupBy(x => new { x.Year, x.Month })
                .Select(g => new MonthlyCompletedUsersDto
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    CompletedUsers = g.Select(x => x.UserId).Distinct().LongCount()
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            var quizIds = quizzes.Select(q => q.Id).ToList();
            var quizAttemptRows = quizIds.Count == 0
                ? new List<QuizAttemptRow>()
                : await dbContext.QuizAttempts
                    .AsNoTracking()
                    .Where(a => quizIds.Contains(a.QuizId))
                    .Select(a => new QuizAttemptRow
                    {
                        QuizId = a.QuizId,
                        Status = a.Status
                    })
                    .ToListAsync(cancellationToken);

            QuizLeaderboardItemDto? mostAttemptedQuiz = null;
            QuizLeaderboardItemDto? mostPassedQuiz = null;
            QuizLeaderboardItemDto? mostFailedQuiz = null;

            if (quizAttemptRows.Count > 0)
            {
                var quizAgg = quizAttemptRows
                    .GroupBy(a => a.QuizId)
                    .Select(g => new
                    {
                        QuizId = g.Key,
                        AttemptCount = g.LongCount(),
                        PassedCount = g.LongCount(x => x.Status == QuizAttemptStatus.Passed),
                        FailedCount = g.LongCount(x => x.Status == QuizAttemptStatus.Failed)
                    })
                    .ToList();

                var quizMap = quizzes.ToDictionary(q => q.Id, q => q);
                var roadmapMap = roadmaps.ToDictionary(r => r.Id, r => r);

                var mostAttempted = quizAgg
                    .OrderByDescending(x => x.AttemptCount)
                    .ThenBy(x => x.QuizId)
                    .FirstOrDefault(x => x.AttemptCount > 0);

                if (mostAttempted is not null && quizMap.TryGetValue(mostAttempted.QuizId, out var meta1))
                {
                    var roadmap = roadmapMap[meta1.RoadmapId];
                    mostAttemptedQuiz = new QuizLeaderboardItemDto
                    {
                        QuizId = meta1.Id,
                        QuizTitle = meta1.Title,
                        RoadmapId = roadmap.Id,
                        RoadmapTitle = roadmap.Title,
                        Count = mostAttempted.AttemptCount
                    };
                }

                var mostPassed = quizAgg
                    .OrderByDescending(x => x.PassedCount)
                    .ThenBy(x => x.QuizId)
                    .FirstOrDefault(x => x.PassedCount > 0);

                if (mostPassed is not null && quizMap.TryGetValue(mostPassed.QuizId, out var meta2))
                {
                    var roadmap = roadmapMap[meta2.RoadmapId];
                    mostPassedQuiz = new QuizLeaderboardItemDto
                    {
                        QuizId = meta2.Id,
                        QuizTitle = meta2.Title,
                        RoadmapId = roadmap.Id,
                        RoadmapTitle = roadmap.Title,
                        Count = mostPassed.PassedCount
                    };
                }

                var mostFailed = quizAgg
                    .OrderByDescending(x => x.FailedCount)
                    .ThenBy(x => x.QuizId)
                    .FirstOrDefault(x => x.FailedCount > 0);

                if (mostFailed is not null && quizMap.TryGetValue(mostFailed.QuizId, out var meta3))
                {
                    var roadmap = roadmapMap[meta3.RoadmapId];
                    mostFailedQuiz = new QuizLeaderboardItemDto
                    {
                        QuizId = meta3.Id,
                        QuizTitle = meta3.Title,
                        RoadmapId = roadmap.Id,
                        RoadmapTitle = roadmap.Title,
                        Count = mostFailed.FailedCount
                    };
                }
            }

            var stats = new ContentManagerStatsDto
            {
                TotalRoadmapsCreated = totalRoadmapsCreated,
                TotalNodesAdded = totalNodesAdded,
                TotalNodeContentsCreated = totalNodeContentsCreated,
                TotalQuizzesCreated = totalQuizzesCreated,
                TotalUsersCompletedRoadmaps = completedUsers.Count,
                TotalUsersInProgressRoadmaps = inProgressUsers.Count,
                TopRoadmapMostLearned = topRoadmapMostLearned,
                CompletedUsersByMonth = completedUsersByMonth,
                MostAttemptedQuiz = mostAttemptedQuiz,
                MostPassedQuiz = mostPassedQuiz,
                MostFailedQuiz = mostFailedQuiz
            };

            return new GetContentManagerStatsResult(stats);
        }

        private sealed class QuizRow
        {
            public long Id { get; set; }
            public string? Title { get; set; }
            public long RoadmapId { get; set; }
        }

        private sealed class ModuleRow
        {
            public long StudyPlanId { get; set; }
            public ModuleStatus? Status { get; set; }
            public DateTime? CompletedAt { get; set; }
            public string UserId { get; set; } = null!;
        }

        private sealed class QuizAttemptRow
        {
            public long QuizId { get; set; }
            public QuizAttemptStatus Status { get; set; }
        }
    }
}
