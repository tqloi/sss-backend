using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.External.Communication.Email;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Enums;

namespace SSS.Infrastructure.Services
{
    public class StudyPlanService(
        IAppDbContext db,
        INotificationService notificationService,
        IEmailJobDispatcher emailJobDispatcher,
        IMailTemplateBuilder mailTemplateBuilder,
        IConfiguration configuration,
        ILogger<StudyPlanService> logger) : IStudyPlanService
    {
        public async Task<StudyPlan> CreatePlanWithModulesAsync(
            string userId, long roadmapId, CancellationToken ct = default)
        {
            // Load all nodes of the roadmap ordered by their sequence
            var nodes = await db.RoadmapNodes
                .Where(n => n.RoadmapId == roadmapId)
                .OrderBy(n => n.OrderNo)
                .ToListAsync(ct);

            if (nodes.Count == 0)
                logger.LogWarning("[StudyPlanService] Roadmap {RoadmapId} has no nodes. Creating plan without modules.", roadmapId);

            var plan = new StudyPlan
            {
                UserId = userId,
                RoadmapId = roadmapId,
                ProfileVersion = 1,
                Strategy = StudyPlanStrategy.Balanced,
                Status = StudyPlanStatus.Ready, // Set to Ready so redirection can happen
                CreatedAt = DateTime.UtcNow
            };

            using var transaction = await db.BeginTransactionAsync(ct);

            try
            {
                // Create the plan
                db.StudyPlans.Add(plan);
                await db.SaveChangesAsync(ct);

                // One module per roadmap node
                if (nodes.Count > 0)
                {
                    var modules = nodes.Select((node, index) => new StudyPlanModule
                    {
                        StudyPlanId = plan.Id,
                        RoadmapNodeId = node.Id,
                        Status = index == 0 ? ModuleStatus.Active : ModuleStatus.Locked,
                        isTaskGenerated = false
                    }).ToList();

                    db.StudyPlanModules.AddRange(modules);
                    await db.SaveChangesAsync(ct);
                }

                logger.LogInformation("[StudyPlanService] StudyPlan {PlanId} created for user {UserId} with {ModuleCount} modules.",
                    plan.Id, userId, nodes.Count);

                await transaction.CommitAsync(ct);
                
                // Trigger SignalR notification after DB is committed
                if (plan.Status.HasValue)
                {
                    await NotifyStatusUpdatedAsync(plan, plan.Status.Value, ct);
                }
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync(ct);
                logger.LogError(ex, "[StudyPlanService] Error creating plan for user {UserId}", userId);
                throw;
            }

            return plan;
        }

        public async Task SetStatusAsync(long studyPlanId, StudyPlanStatus status, CancellationToken ct = default)
        {
            var plan = await db.StudyPlans.FindAsync(new object[] { studyPlanId }, ct);
            if (plan is null)
            {
                logger.LogWarning("[StudyPlanService] StudyPlan {PlanId} not found when setting status {Status}.", studyPlanId, status);
                return;
            }

            plan.Status = status;
            await db.SaveChangesAsync(ct);

            logger.LogInformation("[StudyPlanService] StudyPlan {PlanId} status → {Status}.", studyPlanId, status);

            // Handle Realtime Notification for terminal statuses
            await NotifyStatusUpdatedAsync(plan, status, ct);
        }

        private async Task NotifyStatusUpdatedAsync(StudyPlan plan, StudyPlanStatus status, CancellationToken ct)
        {
            try
            {
                if (status != StudyPlanStatus.Ready && status != StudyPlanStatus.Failed)
                    return;

                string dedupeKey = $"studyPlan:{plan.Id}:{status.ToString().ToLower()}";

                // Idempotency check: don't send if already sent
                var alreadyExists = await db.UserNotifications
                    .AnyAsync(n => n.DedupeKey == dedupeKey, ct);

                if (alreadyExists) return;

                string title = status == StudyPlanStatus.Ready
                    ? "Your study plan is ready!"
                    : "Study plan generation failed";

                string message = status == StudyPlanStatus.Ready
                    ? "We have finished generating your personalized study plan. You can start learning now."
                    : "We encountered an error while building your study plan. Please try again later.";

                await notificationService.CreateAndDispatchAsync(
                    userId: plan.UserId,
                    title: title,
                    content: message,
                    type: NotificationType.System,
                    relatedType: NotificationRelatedType.Plan,
                    relatedId: plan.Id,
                    status: status.ToString(),
                    actionUrl: $"/dashboard/{plan.Id}",
                    dedupeKey: dedupeKey,
                    isPush: false,
                    ct: ct
                );

                if (status == StudyPlanStatus.Ready)
                {
                    await DispatchPlanReadyEmailAsync(plan, ct);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "[StudyPlanService] Failed to dispatch notification for plan {PlanId}", plan.Id);
            }
        }

        private async Task DispatchPlanReadyEmailAsync(StudyPlan plan, CancellationToken ct)
        {
            var user = await db.Users
                .AsNoTracking()
                .Where(u => u.Id == plan.UserId)
                .Select(u => new
                {
                    u.Email,
                    u.FirstName,
                    u.LastName,
                    u.UserName
                })
                .FirstOrDefaultAsync(ct);

            if (user is null || string.IsNullOrWhiteSpace(user.Email))
                return;

            var roadmap = await db.Roadmaps
                .AsNoTracking()
                .Where(r => r.Id == plan.RoadmapId)
                .Select(r => new
                {
                    r.Title
                })
                .FirstOrDefaultAsync(ct);

            var displayName = string.Join(" ", new[] { user.FirstName, user.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x))).Trim();

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = user.UserName ?? "Learner";

            var feBaseUrl = (configuration["Frontend:BaseUrl"] ?? string.Empty).TrimEnd('/');
            var planUrl = string.IsNullOrWhiteSpace(feBaseUrl)
                ? string.Empty
                : $"{feBaseUrl}/dashboard/{plan.Id}";

            var emailBody = await mailTemplateBuilder.BuildPlanReadyEmailAsync(
                studentName: displayName,
                planName: $"Study plan #{plan.Id}",
                roadmapName: roadmap?.Title ?? $"Roadmap {plan.RoadmapId}",
                planUrl: planUrl,
                email: user.Email);

            emailJobDispatcher.DispatchSendEmail(
                to: user.Email,
                subject: "StudySense - Your plan is ready",
                body: emailBody);
        }
    }
}
