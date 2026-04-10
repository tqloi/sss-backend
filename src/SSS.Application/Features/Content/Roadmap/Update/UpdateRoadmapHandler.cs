using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.Roadmap.Update
{
    public sealed class UpdateRoadmapHandler(IAppDbContext dbContext) 
        : IRequestHandler<UpdateRoadmapCommand, UpdateRoadmapResult>
    {
        public async Task<UpdateRoadmapResult> Handle(UpdateRoadmapCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Roadmaps
                .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

            if (entity is null)
            {
                return new UpdateRoadmapResult
                {
                    Success = false,
                    Message = "Roadmap not found.",
                    Data = null
                };
            }

            // Partial update - only update if provided
            if (request.Title is not null)
            {
                entity.Title = request.Title;
            }

            if (request.Description is not null)
            {
                entity.Description = request.Description;
            }

            if (request.Status.HasValue)
            {
                if (entity.Status == RoadmapStatus.Active
                    && request.Status.Value == RoadmapStatus.Disabled)
                {
                    var hasActiveLearners = await dbContext.StudyPlanModules
                        .AsNoTracking()
                        .AnyAsync(
                            m => m.StudyPlan.RoadmapId == entity.Id
                                 && m.Status == ModuleStatus.Active,
                            cancellationToken);

                    if (hasActiveLearners)
                    {
                        return new UpdateRoadmapResult
                        {
                            Success = false,
                            Message = "Cannot disable roadmap because users are currently studying it.",
                            Data = null
                        };
                    }
                }

                entity.Status = request.Status.Value;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateRoadmapResult
            {
                Success = true,
                Message = "Roadmap updated successfully.",
                Data = new RoadmapBasicDTO
                {
                    Id = entity.Id,
                    SubjectId = entity.SubjectId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Status = entity.Status
                }
            };
        }
    }
}
