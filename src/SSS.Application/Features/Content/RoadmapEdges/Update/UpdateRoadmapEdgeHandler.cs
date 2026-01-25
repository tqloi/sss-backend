using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.RoadmapEdges.Update
{
    public sealed class UpdateRoadmapEdgeHandler(IAppDbContext dbContext) 
        : IRequestHandler<UpdateRoadmapEdgeCommand, UpdateRoadmapEdgeResult>
    {
        public async Task<UpdateRoadmapEdgeResult> Handle(UpdateRoadmapEdgeCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.RoadmapEdges
                .FirstOrDefaultAsync(x => x.Id == request.EdgeId && x.RoadmapId == request.RoadmapId, cancellationToken);

            if (entity is null)
            {
                return new UpdateRoadmapEdgeResult
                {
                    Success = false,
                    Message = "Roadmap edge not found or does not belong to this roadmap.",
                    Data = null
                };
            }

            // Partial update
            if (request.EdgeType.HasValue)
            {
                entity.EdgeType = request.EdgeType.Value;
            }

            if (request.OrderNo.HasValue)
            {
                entity.OrderNo = request.OrderNo;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateRoadmapEdgeResult
            {
                Success = true,
                Message = "Roadmap edge updated successfully.",
                Data = new RoadmapEdgeDTO
                {
                    Id = entity.Id,
                    RoadmapId = entity.RoadmapId,
                    FromNodeId = entity.FromNodeId,
                    ToNodeId = entity.ToNodeId,
                    EdgeType = entity.EdgeType,
                    OrderNo = entity.OrderNo
                }
            };
        }
    }
}
