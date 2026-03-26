using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Caching;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Abstractions.Services;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Enums;
using SSS.Infrastructure.Persistence.Sql;

namespace SSS.Infrastructure.Services
{
    public class ModuleService : IModuleService
    {
        private readonly IAppDbContext _context;
        private readonly ICacheService _cacheService;

        public ModuleService(AppDbContext context, ICacheService cacheService)
        {
            _context = context;
            _cacheService = cacheService;
        }

        public async Task CompleteModuleAsync(int moduleId, CancellationToken ct)
        {
            // Lấy module
            var module = await _context.StudyPlanModules
                .FirstOrDefaultAsync(m => m.Id == moduleId, ct);

            if (module == null)
                throw new NotFoundException($"Module {moduleId}");

            // Lấy study plan để biết userId và roadmapId (dùng cho cache key)
            var plan = await _context.StudyPlans
                .FirstOrDefaultAsync(p => p.Id == module.StudyPlanId, ct);

            // Nếu plan hoặc roadmap không tồn tại, có thể do dữ liệu không nhất quán — tùy chọn: log và return (không throw) để tránh crash toàn bộ, hoặc throw để surface lỗi sớm. Ở đây mình chọn throw để dễ phát hiện và fix lỗi data inconsistency.
            var userId = plan?.UserId;
            var roadmapId = plan?.RoadmapId;
            if (userId == null || roadmapId == null)
                throw new InvalidOperationException($"Study plan or roadmap not found for module {moduleId}");

            // Cập nhật trạng thái
            module.Status = ModuleStatus.Completed;

            await _context.SaveChangesAsync(ct);

            // 3️⃣ Invalidate cache toàn bộ study plan
            var cacheKey1 = $"studyplan:roadmap:{userId}:{roadmapId}";
            var cacheKey2 = $"studyplan:id:{module.StudyPlanId}";

            await _cacheService.RemoveAsync(cacheKey1);
            await _cacheService.RemoveAsync(cacheKey2);
        }
    }
}
