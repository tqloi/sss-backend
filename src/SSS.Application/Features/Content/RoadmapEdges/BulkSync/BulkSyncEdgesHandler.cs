using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.Content.RoadmapEdges.BulkSync
{
    public sealed class BulkSyncEdgesHandler(IAppDbContext dbContext) 
        : IRequestHandler<BulkSyncEdgesCommand, BulkSyncEdgesResult>
    {
        public async Task<BulkSyncEdgesResult> Handle(BulkSyncEdgesCommand request, CancellationToken cancellationToken)
        {
            // Validation 1: Check roadmap exists
            var roadmapExists = await dbContext.Roadmaps
                .AnyAsync(r => r.Id == request.RoadmapId, cancellationToken);

            if (!roadmapExists)
            {
                return new BulkSyncEdgesResult
                {
                    Success = false,
                    Message = "Roadmap not found.",
                    Data = null
                };
            }

            // Validation 2: Check for self-loops in payload
            var hasSelfLoops = request.Edges.Any(e => e.FromNodeId == e.ToNodeId);
            if (hasSelfLoops)
            {
                return new BulkSyncEdgesResult
                {
                    Success = false,
                    Message = "Self-loops are not allowed (FromNodeId cannot equal ToNodeId).",
                    Data = null
                };
            }

            // Validation 3: Check for duplicates in payload using (FromNodeId, ToNodeId, EdgeType) as identity
            var payloadGroups = request.Edges
                .GroupBy(e => new { e.FromNodeId, e.ToNodeId, e.EdgeType })
                .Where(g => g.Count() > 1)
                .ToList();

            if (payloadGroups.Any())
            {
                return new BulkSyncEdgesResult
                {
                    Success = false,
                    Message = "Duplicate edges found in payload (same FromNodeId, ToNodeId, and EdgeType).",
                    Data = null
                };
            }

            // Validation 4: Check all nodes exist and belong to this roadmap
            var allNodeIds = request.Edges
                .SelectMany(e => new[] { e.FromNodeId, e.ToNodeId })
                .Distinct()
                .ToList();

            var existingNodeIds = await dbContext.RoadmapNodes
                .Where(n => n.RoadmapId == request.RoadmapId && allNodeIds.Contains(n.Id))
                .Select(n => n.Id)
                .ToListAsync(cancellationToken);

            if (existingNodeIds.Count != allNodeIds.Count)
            {
                return new BulkSyncEdgesResult
                {
                    Success = false,
                    Message = "One or more nodes not found or do not belong to this roadmap.",
                    Data = null
                };
            }

            // Begin sync transaction
            // Strategy: Use (FromNodeId, ToNodeId, EdgeType) as identity for matching

            // Get existing edges for this roadmap
            var existingEdges = await dbContext.RoadmapEdges
                .Where(e => e.RoadmapId == request.RoadmapId)
                .ToListAsync(cancellationToken);

            // Create dictionaries for comparison
            var payloadDict = request.Edges
                .ToDictionary(e => (e.FromNodeId, e.ToNodeId, e.EdgeType));

            var existingDict = existingEdges
                .ToDictionary(e => (e.FromNodeId, e.ToNodeId, e.EdgeType));

            // Determine: Add, Update, Delete
            var toAdd = new List<RoadmapEdge>();
            var toUpdate = new List<RoadmapEdge>();
            var toDelete = new List<RoadmapEdge>();

            // Check payload items
            foreach (var payloadEdge in request.Edges)
            {
                var key = (payloadEdge.FromNodeId, payloadEdge.ToNodeId, payloadEdge.EdgeType);

                if (existingDict.TryGetValue(key, out var existing))
                {
                    // Exists - check if OrderNo changed (update if different)
                    if (existing.OrderNo != payloadEdge.OrderNo)
                    {
                        existing.OrderNo = payloadEdge.OrderNo;
                        toUpdate.Add(existing);
                    }
                }
                else
                {
                    // New edge - add
                    toAdd.Add(new RoadmapEdge
                    {
                        RoadmapId = request.RoadmapId,
                        FromNodeId = payloadEdge.FromNodeId,
                        ToNodeId = payloadEdge.ToNodeId,
                        EdgeType = payloadEdge.EdgeType,
                        OrderNo = payloadEdge.OrderNo
                    });
                }
            }

            // Check existing edges not in payload - delete them
            foreach (var existing in existingEdges)
            {
                var key = (existing.FromNodeId, existing.ToNodeId, existing.EdgeType);
                if (!payloadDict.ContainsKey(key))
                {
                    toDelete.Add(existing);
                }
            }

            // Execute changes
            if (toDelete.Any())
            {
                dbContext.RoadmapEdges.RemoveRange(toDelete);
            }

            if (toAdd.Any())
            {
                dbContext.RoadmapEdges.AddRange(toAdd);
            }

            // toUpdate items are already tracked and modified

            await dbContext.SaveChangesAsync(cancellationToken);

            // Return updated edge list
            var finalEdges = await dbContext.RoadmapEdges
                .Where(e => e.RoadmapId == request.RoadmapId)
                .ToListAsync(cancellationToken);

            var result = finalEdges.Select(e => new RoadmapEdgeDTO
            {
                Id = e.Id,
                RoadmapId = e.RoadmapId,
                FromNodeId = e.FromNodeId,
                ToNodeId = e.ToNodeId,
                EdgeType = e.EdgeType,
                OrderNo = e.OrderNo
            }).ToList();

            return new BulkSyncEdgesResult
            {
                Success = true,
                Message = $"Bulk sync completed: {toAdd.Count} added, {toUpdate.Count} updated, {toDelete.Count} deleted.",
                Data = result
            };
        }
    }
}
