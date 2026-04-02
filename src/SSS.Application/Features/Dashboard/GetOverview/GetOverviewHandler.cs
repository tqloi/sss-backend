using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudySessions.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Dashboard.GetOverview
{
    public class GetOverviewHandler(IAppDbContext context)
        : IRequestHandler<GetOverviewQuery, GetOverviewResult>
    {
        public async Task<GetOverviewResult> Handle(GetOverviewQuery req, CancellationToken ct)
        {
            var plan = await context.StudyPlans
                .Include(p => p.Roadmap)
                .Include(p => p.Modules)
                .FirstOrDefaultAsync(p => p.Id == req.StudyPlanId && p.UserId == req.UserId, ct);

            if (plan == null)
            {
                throw new NotFoundException($"StudyPlan {req.StudyPlanId} not found");
            }

            var roadmapTitle = plan.Roadmap.Title ?? "Study Plan";

            // Calculation for ProgressPercentage
            var totalModules = plan.Modules.Count;
            var completedModules = plan.Modules.Count(m => m.Status == ModuleStatus.Completed);
            var progress = totalModules > 0 ? ((double)completedModules / totalModules) * 100 : 0;

            // Gamification stats
            var gamification = await context.UserGamifications
                .FirstOrDefaultAsync(g => g.UserId == req.UserId, ct);

            var totalXp = gamification?.TotalExp ?? 0;
            var currentStreak = gamification?.CurrentStreak ?? 0;

            // TodaysFocus and UpcomingTasks
            var pendingTasks = await context.TaskItems
                .Include(t => t.StudyPlanModule)
                .ThenInclude(m => m.RoadmapNode)
                .Where(t => t.StudyPlanModule.StudyPlanId == req.StudyPlanId && t.Status != Domain.Enums.TaskStatus.Completed)
                .OrderBy(t => t.StudyPlanModule.Id)
                .ThenBy(t => t.Id)
                .Take(4)
                .ToListAsync(ct);

            OverviewTaskDto? todaysFocus = null;
            var upcomingTasks = new List<OverviewTaskDto>();

            if (pendingTasks.Any())
            {
                var first = pendingTasks.First();
                todaysFocus = new OverviewTaskDto
                {
                    TaskId = first.Id,
                    ModuleId = first.StudyPlanModuleId,
                    ModuleTitle = first.StudyPlanModule.RoadmapNode.Title ?? "Active Module",
                    TaskTitle = first.Title,
                    Description = first.Description,
                    EstimatedMinutes = first.EstimatedDurationSeconds / 60
                };

                upcomingTasks = pendingTasks.Skip(1).Select(t => new OverviewTaskDto
                {
                    TaskId = t.Id,
                    ModuleId = t.StudyPlanModuleId,
                    ModuleTitle = t.StudyPlanModule.RoadmapNode.Title ?? "Upcoming Module",
                    TaskTitle = t.Title,
                    Description = t.Description,
                    EstimatedMinutes = t.EstimatedDurationSeconds / 60
                }).ToList();
            }

            // Recent Sessions (Max 2)
            var recentSessionRecords = await context.StudySessions
                .Where(s => s.UserId == req.UserId && s.StudyPlanId == req.StudyPlanId && s.Status == SessionStatus.Completed)
                .OrderByDescending(s => s.EndAt ?? s.StartAt)
                .Take(2)
                .Select(s => new
                {
                    s.Id,
                    DurationSeconds = s.ActualDurationSeconds ?? 0,
                    TasksCompletedCount = s.TasksCompletedCount ?? 0,
                    TotalTasks = s.TotalTasks ?? 0,
                    Date = s.EndAt ?? s.StartAt,
                    s.SelfRating,
                    NodeTitle = context.StudyPlanModules
                                    .Where(m => m.Id == s.StudyPlanModuleId)
                                    .Select(m => m.RoadmapNode.Title)
                                    .FirstOrDefault()
                })
                .ToListAsync(ct);

            var recentSessions = recentSessionRecords.Select(s => new RecentSessionDto
            {
                Id = s.Id,
                DurationSeconds = s.DurationSeconds,
                TasksCompleted = s.TasksCompletedCount,
                TotalTasks = s.TotalTasks,
                Date = s.Date.ToString("dd/MM/yyyy"),
                Rating = s.SelfRating,
                NodeTitle = s.NodeTitle
            });

            return new GetOverviewResult
            {
                Success = true,
                Message = "Overview fetched successfully",
                Data = new OverviewStudyPlanDto
                {
                    StudyPlanId = plan.Id,
                    RoadmapTitle = roadmapTitle,
                    ProgressPercentage = Math.Round(progress, 1),
                    TotalXpEarned = totalXp,
                    StudyStreakDays = currentStreak,
                    TodaysFocus = todaysFocus,
                    UpcomingTasks = upcomingTasks,
                    RecentSessions = recentSessions
                }
            };
        }
    }
}
