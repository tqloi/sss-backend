using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.Content.RoadmapEdges.Create
{
    public sealed class CreateRoadmapEdgeHandler(IAppDbContext dbContext) 
        : IRequestHandler<CreateRoadmapEdgeCommand, CreateRoadmapEdgeResult>
    {
        public async Task<CreateRoadmapEdgeResult> Handle(CreateRoadmapEdgeCommand request, CancellationToken cancellationToken)
        {
            // Validation 1: Self-loop check
            if (request.FromNodeId == request.ToNodeId)
            {
                return new CreateRoadmapEdgeResult
                {
                    Success = false,
                    Message = "Self-loops are not allowed (FromNodeId cannot equal ToNodeId).",
                    Data = null
                };
            }

            // Validation 2: Both nodes must exist and belong to the same roadmap
            var fromNode = await dbContext.RoadmapNodes
                .FirstOrDefaultAsync(n => n.Id == request.FromNodeId && n.RoadmapId == request.RoadmapId, cancellationToken);

            var toNode = await dbContext.RoadmapNodes
                .FirstOrDefaultAsync(n => n.Id == request.ToNodeId && n.RoadmapId == request.RoadmapId, cancellationToken);

            if (fromNode is null || toNode is null)
            {
                return new CreateRoadmapEdgeResult
                {
                    Success = false,
                    Message = "One or both nodes not found or do not belong to this roadmap.",
                    Data = null
                };
            }

            // Validation 3: Check for duplicate edge (RoadmapId, FromNodeId, ToNodeId, EdgeType)
            var duplicateExists = await dbContext.RoadmapEdges
                .AnyAsync(e => e.RoadmapId == request.RoadmapId 
                    && e.FromNodeId == request.FromNodeId 
                    && e.ToNodeId == request.ToNodeId 
                    && e.EdgeType == request.EdgeType, cancellationToken);

            if (duplicateExists)
            {
                return new CreateRoadmapEdgeResult
                {
                    Success = false,
                    Message = "Duplicate edge already exists (same FromNodeId, ToNodeId, and EdgeType).",
                    Data = null
                };
            }

            var entity = new RoadmapEdge
            {
                RoadmapId = request.RoadmapId,
                FromNodeId = request.FromNodeId,
                ToNodeId = request.ToNodeId,
                EdgeType = request.EdgeType,
                OrderNo = request.OrderNo
            };

            dbContext.RoadmapEdges.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateRoadmapEdgeResult
            {
                Success = true,
                Message = "Roadmap edge created successfully.",
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
