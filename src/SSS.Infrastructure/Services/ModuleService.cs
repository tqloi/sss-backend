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
        private readonly ISurveyJobDispatcher _surveyJobDispatcher;
        private readonly IEmailJobDispatcher _emailJobDispatcher;
        private readonly IMailTemplateBuilder _mailTemplateBuilder;
        private readonly IConfiguration _configuration;

        public ModuleService(
            AppDbContext context,
            ICacheService cacheService,
            IStudyEventRepository studyEvent,
            IMapper mapper,
            IPipeLine pipeLine,
            ISurveyJobDispatcher surveyJobDispatcher,
            IEmailJobDispatcher emailJobDispatcher,
            IMailTemplateBuilder mailTemplateBuilder,
            IConfiguration configuration)
        {
            _context = context;
            _cacheService = cacheService;
            _studyEventRepository = studyEvent;
            this.mapper = mapper;
            this.pipeLine = pipeLine;
            _surveyJobDispatcher = surveyJobDispatcher;
            _emailJobDispatcher = emailJobDispatcher;
            _mailTemplateBuilder = mailTemplateBuilder;
            _configuration = configuration;
        }

        public async Task CompleteModuleAsync(int moduleId, CancellationToken ct)
        {
            // Lấy module
            var module = await _context.StudyPlanModules
                .Include(x => x.RoadmapNode) // eager load để lấy title khi gửi email
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

            // 3️⃣ Invalidate cache toàn bộ study plan
            var cacheKey1 = $"studyplan:roadmap:{userId}:{roadmapId}";
            var cacheKey2 = $"studyplan:id:{module.StudyPlanId}";

            await _cacheService.RemoveAsync(cacheKey1);
            await _cacheService.RemoveAsync(cacheKey2);

            await _context.SaveChangesAsync(ct);

            // Ready data for behavior generation
            var studyEvents = (await _studyEventRepository.GetByUserIdAsync(userId, moduleId.ToString()))
                .OrderByDescending(e => e.EventTimestamp)
                .Take(100)
                .ToList();

            var quizAttempts = await _context.QuizAttempts
               .AsNoTracking()
               .Include(a => a.Quiz)
               .Include(a => a.Answers)
               .Where(a => a.UserId == userId && a.Quiz.RoadmapNodeId == module.RoadmapNodeId)
               .OrderByDescending(a => a.SubmittedAt ?? a.StartedAt)
               .Take(20)
               .ToListAsync(ct);

            var sessions = await _context.StudySessions
                .AsNoTracking()
                .Include(s => s.SessionTasks)
                    .ThenInclude(st => st.TaskItem)
                .Where(s => s.UserId == userId &&
                            (s.StudyPlanModuleId == moduleId ||
                             s.SessionTasks.Any(st => st.TaskItem.StudyPlanModuleId == moduleId)))
                .OrderByDescending(s => s.EndAt ?? s.StartAt)
                .Take(20)
                .ToListAsync(ct);

            var completedTaskCount = module.Tasks.Count(t => t.Status == SSS.Domain.Enums.TaskStatus.Completed || t.CompletedAt.HasValue);
            var completedQuizCount = quizAttempts.Count(a => a.Status != QuizAttemptStatus.InProgress || a.SubmittedAt.HasValue);

            var moduleDto = mapper.Map<BehaviorModuleDto>(module);
            var sessionDtos = mapper.Map<List<BehaviorSessionDto>>(sessions);

            foreach (var sessionDto in sessionDtos)
            {
                sessionDto.Tasks = sessionDto.Tasks
                    .Where(t => t.StudyPlanModuleId == moduleId)
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
                Module = moduleDto,
                Sessions = sessionDtos,
                QuizAttempts = quizAttemptDtos,
                StudyEvents = studyEventDetails,
                StudyEventSummary = studyEventSummary,

            };

            var behaviorContextJson = JsonSerializer.Serialize(behaviorContext);

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

            _surveyJobDispatcher.DispatchModuleBehaviorInsight(plan!.Id, moduleId, userId);

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
    }
}
