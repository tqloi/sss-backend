using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Enums;

namespace SSS.Application.Features.AdminAnalytics.GetOverview
{
    public sealed class GetAdminDashboardOverviewHandler(IAppDbContext dbContext)
        : IRequestHandler<GetAdminDashboardOverviewQuery, GetAdminDashboardOverviewResult>
    {
        public async Task<GetAdminDashboardOverviewResult> Handle(GetAdminDashboardOverviewQuery request, CancellationToken ct)
        {
            var totalUsers = await dbContext.Users
                .AsNoTracking()
                .LongCountAsync(ct);

            var activeUsers = await dbContext.Users
                .AsNoTracking()
                .LongCountAsync(u => u.IsActive != false, ct);

            var inactiveUsers = totalUsers - activeUsers;

            var userRoleRows = await (
                from ur in dbContext.UserRoles.AsNoTracking()
                join r in dbContext.Roles.AsNoTracking() on ur.RoleId equals r.Id
                where r.Name != null
                select new
                {
                    ur.UserId,
                    RoleName = r.Name!
                })
                .ToListAsync(ct);

            var roleCountMap = userRoleRows
                .GroupBy(x => NormalizeRoleName(x.RoleName))
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(x => x.UserId).Distinct().LongCount());

            var adminCount = roleCountMap.GetValueOrDefault("admin", 0);
            var analystCount = roleCountMap.GetValueOrDefault("analyst", 0);
            var contentManagerCount = roleCountMap.GetValueOrDefault("contentmanager", 0);
            var userCount = roleCountMap.GetValueOrDefault("user", 0);

            var assignedContentManagers = await dbContext.ContentManagerSubjects
                .AsNoTracking()
                .Where(x => x.IsActive)
                .Select(x => x.ContentManagerId)
                .Distinct()
                .LongCountAsync(ct);

            var totalCategories = await dbContext.LearningCategories
                .AsNoTracking()
                .LongCountAsync(ct);

            var activeCategories = await dbContext.LearningCategories
                .AsNoTracking()
                .LongCountAsync(x => x.IsActive, ct);

            var totalSubjects = await dbContext.LearningSubjects
                .AsNoTracking()
                .LongCountAsync(ct);

            var activeSubjects = await dbContext.LearningSubjects
                .AsNoTracking()
                .LongCountAsync(x => x.IsActive, ct);

            var totalLatestRoadmaps = await dbContext.Roadmaps
                .AsNoTracking()
                .LongCountAsync(r => r.IsLatest, ct);

            var draftRoadmaps = await dbContext.Roadmaps
                .AsNoTracking()
                .LongCountAsync(r => r.IsLatest && r.Status == RoadmapStatus.Draft, ct);

            var activeRoadmaps = await dbContext.Roadmaps
                .AsNoTracking()
                .LongCountAsync(r => r.IsLatest && r.Status == RoadmapStatus.Active, ct);

            var archivedRoadmaps = await dbContext.Roadmaps
                .AsNoTracking()
                .LongCountAsync(r => r.IsLatest && r.Status == RoadmapStatus.Archived, ct);

            var knownStatusRoadmaps = draftRoadmaps + activeRoadmaps + archivedRoadmaps;
            var unknownRoadmaps = Math.Max(0, totalLatestRoadmaps - knownStatusRoadmaps);

            var result = new AdminDashboardOverviewDto
            {
                Summary = new AdminDashboardSummaryDto
                {
                    TotalUsers = totalUsers,
                    ActiveUsers = activeUsers,
                    InactiveUsers = inactiveUsers,
                    ContentManagers = contentManagerCount,
                    AssignedContentManagers = assignedContentManagers,
                    Analysts = analystCount,
                    TotalLatestRoadmaps = totalLatestRoadmaps,
                    ActiveLatestRoadmaps = activeRoadmaps
                },
                RoleDistribution = new List<RoleDistributionItemDto>
                {
                    new()
                    {
                        Role = "Admin",
                        Count = adminCount,
                        Percentage = CalculateRate(adminCount, totalUsers)
                    },
                    new()
                    {
                        Role = "Analyst",
                        Count = analystCount,
                        Percentage = CalculateRate(analystCount, totalUsers)
                    },
                    new()
                    {
                        Role = "Content Manager",
                        Count = contentManagerCount,
                        Percentage = CalculateRate(contentManagerCount, totalUsers)
                    },
                    new()
                    {
                        Role = "User",
                        Count = userCount,
                        Percentage = CalculateRate(userCount, totalUsers)
                    }
                },
                LearningCoverage = new LearningCoverageDto
                {
                    ActiveCategories = activeCategories,
                    TotalCategories = totalCategories,
                    ActiveSubjects = activeSubjects,
                    TotalSubjects = totalSubjects,
                    CategoryActivationRate = CalculateRate(activeCategories, totalCategories),
                    SubjectActivationRate = CalculateRate(activeSubjects, totalSubjects),
                    AverageSubjectsPerCategory = totalCategories == 0
                        ? 0
                        : Math.Round((double)totalSubjects / totalCategories, 1)
                },
                RoadmapStatusBreakdown = new RoadmapStatusBreakdownDto
                {
                    Active = activeRoadmaps,
                    Draft = draftRoadmaps,
                    Archived = archivedRoadmaps,
                    Unknown = unknownRoadmaps,
                    Total = totalLatestRoadmaps,
                    ActiveRate = CalculateRate(activeRoadmaps, totalLatestRoadmaps)
                }
            };

            return new GetAdminDashboardOverviewResult(result);
        }

        private static string NormalizeRoleName(string roleName)
        {
            return roleName.Replace(" ", string.Empty).Trim().ToLowerInvariant();
        }

        private static int CalculateRate(long part, long total)
        {
            if (total <= 0)
            {
                return 0;
            }

            return (int)Math.Round((double)part / total * 100, MidpointRounding.AwayFromZero);
        }
    }
}
