using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Application.Features.StudyPlans.StudyPlans.Common;
using SSS.Domain.Entities.Planning;
using SSS.Domain.Enums;

namespace SSS.Application.Features.StudyPlans.StudyPlans.CreateStudyPlan
{
    public class CreateStudyPlanHandler(
        IAppDbContext context,
        IMapper mapper
        ) : IRequestHandler<CreateStudyPlanCommand, CreateStudyPlanResult>
    {
        public async Task<CreateStudyPlanResult> Handle(CreateStudyPlanCommand req, CancellationToken ct)
        {
            if (await context.StudyPlans.AnyAsync(
                sp => sp.UserId == req.UserId &&
                sp.RoadmapId == req.RoadmapId, ct))
            {
                throw new ConflictException(
                    "Study plan already exists for this roadmap"
                );
            }

            // Lấy roadmap và các nodes của roadmap từ database
            var roadmap = await context.Roadmaps
                .Include(r => r.Nodes)
                .FirstOrDefaultAsync(r => r.Id == req.RoadmapId, ct);

            if (roadmap == null)
            {
                throw new NotFoundException($"Roadmap with Id {req.RoadmapId} not found");
            }

            // 2. Create aggregate
            var studyPlan = new StudyPlan
            {
                UserId = req.UserId,
                RoadmapId = req.RoadmapId,
                Status = StudyPlanStatus.Active,
                CreatedAt = DateTime.UtcNow,
                Modules = roadmap.Nodes.Select(node => new StudyPlanModule
                {
                    RoadmapNodeId = node.Id,
                    Status = ModuleStatus.Locked
                }).ToList()
            };

            // 3. Save ONCE (EF tự transaction)
            context.StudyPlans.Add(studyPlan);
            await context.SaveChangesAsync(ct);

            // 4. ProjectTo DTO
            var dto = await context.StudyPlans
                .AsNoTracking()
                .Where(sp => sp.Id == studyPlan.Id)
                .ProjectTo<StudyPlanDto>(mapper.ConfigurationProvider)
                .FirstAsync(ct);

            return new CreateStudyPlanResult
            {
                Success = true,
                Message = "Study plan created successfully",
                Data = dto
            };
        }
    }
}
