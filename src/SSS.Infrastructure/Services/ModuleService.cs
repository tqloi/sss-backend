using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SSS.Application.Abstractions.Background;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.External.Communication.Email;
using SSS.Application.Abstractions.Persistence.Mongo.Interfaces;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.AI.CreateAiAddBehaviorDb;
using SSS.Domain.Enums;
using SSS.Infrastructure.Persistence.Sql;
using System.Text.Json;

namespace SSS.Infrastructure.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IAppDbContext _context;
        private readonly ICacheService _cacheService;
        private readonly IStudyEventRepository _studyEventRepository;
        private readonly IMapper mapper;
        private readonly IPipeLine pipeLine;
        private readonly IEmailJobDispatcher _emailJobDispatcher;
        private readonly IMailTemplateBuilder _mailTemplateBuilder;
        private readonly IConfiguration _configuration;
        private readonly INotificationService _notificationService;

        public ModuleService(
            AppDbContext context,
            ICacheService cacheService,
            IStudyEventRepository studyEvent,
            IMapper mapper,
            IPipeLine pipeLine,
            IEmailJobDispatcher emailJobDispatcher,
            IMailTemplateBuilder mailTemplateBuilder,
            IConfiguration configuration,
            INotificationService notificationService)
        {
            _context = context;
            _cacheService = cacheService;
            _studyEventRepository = studyEvent;
            this.mapper = mapper;
            this.pipeLine = pipeLine;
            _emailJobDispatcher = emailJobDispatcher;
            _mailTemplateBuilder = mailTemplateBuilder;
            _configuration = configuration;
            _notificationService = notificationService;
        }

        public async Task CompleteModuleAsync(int moduleId, CancellationToken ct)
        {
            // 1 ============================================================
            // Lấy module
            var module = await _context.StudyPlanModules
                .Include(x => x.RoadmapNode) // eager load để lấy title khi gửi email
                .Include(x => x.Tasks)
                .FirstOrDefaultAsync(m => m.Id == moduleId, ct);

            if (module == null)
                throw new NotFoundException($"Module {moduleId}");

            // Lấy study plan để biết userId và roadmapId (dùng cho cache key)
            var plan = await _context.StudyPlans
                .Include(x => x.Roadmap) // eager load roadmap để lấy title khi gửi email
                .FirstOrDefaultAsync(p => p.Id == module.StudyPlanId, ct);

            if (plan == null)
                throw new NotFoundException($"Study plan for module {moduleId}");

            // Nếu plan hoặc roadmap không tồn tại, có thể do dữ liệu không nhất quán — tùy chọn: log và return (không throw) để tránh crash toàn bộ, hoặc throw để surface lỗi sớm. Ở đây mình chọn throw để dễ phát hiện và fix lỗi data inconsistency.
            var userId = plan?.UserId;
            var roadmapId = plan?.RoadmapId;
            if (userId == null || roadmapId == null)
                throw new InvalidOperationException($"Study plan or roadmap not found for module {moduleId}");

            // Cập nhật trạng thái module và mở khóa module tiếp theo nếu có
            module.Status = ModuleStatus.Completed;

            // Đánh dấu tất cả task chưa completed thành Skipped
            foreach (var task in module.Tasks.Where(t => t.Status != Domain.Enums.TaskStatus.Completed))
            {
                task.Status = Domain.Enums.TaskStatus.Skipped;
            }

            var modules = await _context.StudyPlanModules
                .Where(m => m.StudyPlanId == module.StudyPlanId)
                .OrderBy(m => m.Id) // tạm dùng Id làm thứ tự
                .ToListAsync(ct);

            var index = modules.FindIndex(m => m.Id == moduleId);

            if (index >= 0 && index < modules.Count - 1)
            {
                var nextModule = modules[index + 1];

                if (nextModule.Status == ModuleStatus.Locked)
                {
                    nextModule.Status = ModuleStatus.Active;
                }
            }

            // ✅ Nếu tất cả modules Completed/Skipped → gửi notification nhắc đánh giá roadmap
            var allModulesDone = modules.All(m =>
                m.Status == ModuleStatus.Completed || m.Status == ModuleStatus.Skipped);

            var shouldNotify = false;

            if (plan!.Status != StudyPlanStatus.Completed && allModulesDone)
            {
                plan.Status = StudyPlanStatus.Completed;
                shouldNotify = true;
            }

            await _context.SaveChangesAsync(ct);

            if (shouldNotify)
            {
                await _notificationService.CreateAndDispatchAsync(
                    userId: userId,
                    title: "🎉 Bạn đã hoàn thành lộ trình!",
                    content: $"Bạn vừa hoàn thành toàn bộ lộ trình '{plan.Roadmap.Title}'. Hãy để lại đánh giá để giúp cộng đồng nhé!",
                    type: NotificationType.Achievement,
                    relatedType: NotificationRelatedType.Roadmap,
                    relatedId: plan.RoadmapId,
                    //actionUrl: $"/roadmaps/{plan.RoadmapId}/review",
                    dedupeKey: $"roadmap-complete-review-{plan.Id}",
                    ct: ct);
            }

            // 3️⃣ Invalidate cache toàn bộ study plan
            var cacheKey1 = $"studyplan:roadmap:{userId}:{roadmapId}";
            var cacheKey2 = $"studyplan:id:{module.StudyPlanId}";

            await _cacheService.RemoveAsync(cacheKey1);
            await _cacheService.RemoveAsync(cacheKey2);

            // 2 ==========================================================
            // Ready data for behavior generation
            var studyEvents = (await _studyEventRepository.GetByUserIdAsync(userId, moduleId.ToString()))
                .OrderByDescending(e => e.EventTimestamp)
                .Take(100)
                .ToList();

            var recentNodeIds = await GetRecentCompletedRoadmapNodeIdsAsync(plan.Id, plan.RoadmapId, module.RoadmapNodeId, 3, ct);

            var recentModuleIds = await _context.StudyPlanModules
                .AsNoTracking()
                .Where(m => m.StudyPlanId == module.StudyPlanId && recentNodeIds.Contains(m.RoadmapNodeId))
                .Select(m => m.Id)
                .ToListAsync(ct);

            var quizAttempts = await _context.QuizAttempts
               .AsNoTracking()
               .Include(a => a.Quiz)
               .Include(a => a.Answers)
               .Where(a => a.UserId == userId && recentNodeIds.Contains(a.Quiz.RoadmapNodeId))
               .OrderByDescending(a => a.SubmittedAt ?? a.StartedAt)
               .Take(50)
               .ToListAsync(ct);

            var sessions = await _context.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                .Where(s => s.UserId == userId &&
                            ((s.StudyPlanModuleId.HasValue && recentModuleIds.Contains(s.StudyPlanModuleId.Value)) ||
                             s.SessionTasks.Any(st => recentModuleIds.Contains(st.TaskItem.StudyPlanModuleId))))
                .OrderByDescending(s => s.EndAt ?? s.StartAt)
                .Take(20)
                .ToListAsync(ct);

            var recentSessionTaskSnapshots = await _context.SessionTasks
                .AsNoTracking()
                .Where(st => st.StudySession.UserId == userId
                             && recentModuleIds.Contains(st.TaskItem.StudyPlanModuleId))
                .Select(st => new
                {
                    st.TaskId,
                    st.Status,
                    st.EndTimeUtc,
                    st.TaskItem.CompletedAt,
                    st.TaskItem.ScheduledDate
                })
                .ToListAsync(ct);

            var completedSessionTasks = recentSessionTaskSnapshots
                .Where(st => st.Status == "COMPLETED" && (st.EndTimeUtc.HasValue || st.CompletedAt.HasValue))
                .GroupBy(st => st.TaskId)
                .Select(g => g
                    .OrderByDescending(x => x.EndTimeUtc ?? x.CompletedAt ?? DateTime.MinValue)
                    .First())
                .ToList();

            var completedTaskCount = completedSessionTasks.Count;
            var onTimeCompletedTaskCount = completedSessionTasks
                .Count(st => (st.EndTimeUtc ?? st.CompletedAt) <= st.ScheduledDate);
            var lateCompletedTaskCount = completedTaskCount - onTimeCompletedTaskCount;
            var completedQuizCount = quizAttempts.Count(a => a.Status != QuizAttemptStatus.InProgress || a.SubmittedAt.HasValue);

            var moduleDto = mapper.Map<BehaviorModuleDto>(module);
            var sessionDtos = mapper.Map<List<BehaviorSessionDto>>(sessions);

            foreach (var sessionDto in sessionDtos)
            {
                sessionDto.Tasks = sessionDto.Tasks
                    .Where(t => recentModuleIds.Contains(t.StudyPlanModuleId))
                    .ToList();
            }
            var quizAttemptDtos = mapper.Map<List<BehaviorQuizAttemptDto>>(quizAttempts);

            var studyEventDetails = studyEvents.Select(e => new
            {
                e.SessionId,
                EventType = e.EventType.ToString(),
                EventCategory = e.EventCategory.ToString(),
                ContentMode = e.ContentMode.ToString(),
                e.EventTimestamp,
                e.Payload,
                e.DeviceInfo
            }).ToList();

            var studyEventSummary = new
            {
                TotalEvents = studyEvents.Count,
                EventTypeCounts = studyEvents
                    .GroupBy(e => e.EventType.ToString())
                    .ToDictionary(g => g.Key, g => g.Count()),
                EventCategoryCounts = studyEvents
                    .GroupBy(e => e.EventCategory.ToString())
                    .ToDictionary(g => g.Key, g => g.Count()),
                ContentModeCounts = studyEvents
                    .GroupBy(e => e.ContentMode.ToString())
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            var behaviorContext = new
            {
                NodeScope = new
                {
                    CurrentModuleId = module.Id,
                    CurrentNodeId = module.RoadmapNodeId,
                    RecentNodeIds = recentNodeIds
                },
                Module = moduleDto,
                Sessions = sessionDtos,
                QuizAttempts = quizAttemptDtos,
                LearningSummary = new
                {
                    CompletedTaskCount = completedTaskCount,
                    OnTimeCompletedTaskCount = onTimeCompletedTaskCount,
                    LateCompletedTaskCount = lateCompletedTaskCount,
                    CompletedQuizCount = completedQuizCount
                },
                StudyEvents = studyEventDetails,
                StudyEventSummary = studyEventSummary,

            };

            var behaviorContextJson = JsonSerializer.Serialize(behaviorContext);
            Console.WriteLine(behaviorContextJson);

            var result = await pipeLine.GenerateBehaviorResultAsync(behaviorContextJson, ct);

            if (result is null)
            {
                throw new Exception("Failed to generate behavior result.");
            }

            var chunks = new List<(string Text, string? Source)>
            {
                (result, "user_behavior")
            };

            var vectorStudyPlanId = plan?.Id.ToString() ?? module.StudyPlanId.ToString();
            await pipeLine.IngestBehaviorAsync(vectorStudyPlanId, userId, moduleId.ToString(), chunks, ct);

            await DispatchModuleCompletedEmailAsync(
                userId: userId,
                studyPlanId: plan.Id,
                moduleName: module.RoadmapNode.Title,
                roadmapName: plan.Roadmap.Title,
                ct: ct);
        }

        private async Task DispatchModuleCompletedEmailAsync(
            string userId,
            long studyPlanId,
            string moduleName,
            string roadmapName,
            CancellationToken ct)
        {
            var user = await _context.Users
                .AsNoTracking()
                .Where(u => u.Id == userId)
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

            var displayName = string.Join(" ", new[] { user.FirstName, user.LastName }
                .Where(x => !string.IsNullOrWhiteSpace(x))).Trim();

            if (string.IsNullOrWhiteSpace(displayName))
                displayName = user.UserName ?? "Learner";

            var feBaseUrl = (_configuration["Frontend:BaseUrl"] ?? string.Empty).TrimEnd('/');
            var roadmapUrl = string.IsNullOrWhiteSpace(feBaseUrl)
                ? string.Empty
                : $"{feBaseUrl}/study-plans/{studyPlanId}";

            var emailBody = await _mailTemplateBuilder.BuildModuleCompletedEmailAsync(
                studentName: displayName,
                moduleName: moduleName,
                roadmapName: roadmapName,
                roadmapUrl: roadmapUrl,
                email: user.Email);

            _emailJobDispatcher.DispatchSendEmail(
                to: user.Email,
                subject: "StudySense - Module Completed",
                body: emailBody);
        }

        private async Task<List<long>> GetRecentCompletedRoadmapNodeIdsAsync(long studyPlanId, long roadmapId, long currentNodeId, int take, CancellationToken ct)
        {
            var completedNodeIds = (await _context.StudyPlanModules
                .AsNoTracking()
                .Where(m => m.StudyPlanId == studyPlanId && m.Status == ModuleStatus.Completed)
                .Select(m => m.RoadmapNodeId)
                .Distinct()
                .ToListAsync(ct))
                .ToHashSet();

            var recentNodeIds = new List<long>();

            if (completedNodeIds.Contains(currentNodeId))
            {
                recentNodeIds.Add(currentNodeId);
            }

            var visited = new HashSet<long> { currentNodeId };
            var currentLayer = new HashSet<long> { currentNodeId };

            while (recentNodeIds.Count < take && currentLayer.Count > 0)
            {
                var incomingEdges = await _context.RoadmapEdges
                    .AsNoTracking()
                    .Where(e => e.RoadmapId == roadmapId
                                && (e.EdgeType == EdgeType.Recommended || e.EdgeType == EdgeType.Next)
                                && currentLayer.Contains(e.ToNodeId))
                    .OrderBy(e => e.EdgeType == EdgeType.Recommended ? 0 : 1)
                    .ThenBy(e => e.OrderNo ?? int.MaxValue)
                    .ThenBy(e => e.Id)
                    .ToListAsync(ct);

                var nextLayer = new HashSet<long>();

                foreach (var fromNodeId in incomingEdges.Select(e => e.FromNodeId).Distinct())
                {
                    if (!visited.Add(fromNodeId))
                    {
                        continue;
                    }

                    if (completedNodeIds.Contains(fromNodeId))
                    {
                        recentNodeIds.Add(fromNodeId);
                    }

                    nextLayer.Add(fromNodeId);

                    if (recentNodeIds.Count >= take)
                    {
                        break;
                    }
                }

                currentLayer = nextLayer;
            }

            return recentNodeIds;
        }
    }
}
