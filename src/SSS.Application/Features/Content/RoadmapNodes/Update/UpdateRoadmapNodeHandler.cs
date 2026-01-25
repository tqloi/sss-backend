using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.RoadmapNodes.Update
{
    public sealed class UpdateRoadmapNodeHandler(IAppDbContext dbContext) 
        : IRequestHandler<UpdateRoadmapNodeCommand, UpdateRoadmapNodeResult>
    {
        public async Task<UpdateRoadmapNodeResult> Handle(UpdateRoadmapNodeCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.RoadmapNodes
                .FirstOrDefaultAsync(x => x.Id == request.NodeId && x.RoadmapId == request.RoadmapId, cancellationToken);

            if (entity is null)
            {
                return new UpdateRoadmapNodeResult
                {
                    Success = false,
                    Message = "Roadmap node not found or does not belong to this roadmap.",
                    Data = null
                };
            }

            // Partial update
            if (request.Title is not null)
            {
                entity.Title = request.Title;
            }

            if (request.Description is not null)
            {
                entity.Description = request.Description;
            }

            if (request.Difficulty.HasValue)
            {
                entity.Difficulty = request.Difficulty;
            }

            if (request.OrderNo.HasValue)
            {
                entity.OrderNo = request.OrderNo;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateRoadmapNodeResult
            {
                Success = true,
                Message = "Roadmap node updated successfully.",
                Data = new RoadmapNodeDTO
                {
                    Id = entity.Id,
                    RoadmapId = entity.RoadmapId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Difficulty = entity.Difficulty,
                    OrderNo = entity.OrderNo
                }
            };
        }
    }
}
