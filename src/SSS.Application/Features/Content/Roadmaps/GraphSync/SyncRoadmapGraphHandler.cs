using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmaps.Common;
using SSS.Domain.Entities.Content;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.Roadmaps.GraphSync;

public sealed class SyncRoadmapGraphHandler(IAppDbContext dbContext) 
    : IRequestHandler<SyncRoadmapGraphCommand, SyncRoadmapGraphResult>
{
    public async Task<SyncRoadmapGraphResult> Handle(SyncRoadmapGraphCommand request, CancellationToken cancellationToken)
    {
        var roadmapId = request.RoadmapId;

        // Start transaction
        await using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Validate roadmap exists
            var roadmap = await dbContext.Roadmaps
                .FirstOrDefaultAsync(r => r.Id == roadmapId, cancellationToken);

            if (roadmap == null)
            {
                return new SyncRoadmapGraphResult
                {
                    Success = false,
                    Message = $"Roadmap with ID {roadmapId} not found.",
                    Data = null
                };
            }

            // 2. Load current DB snapshot
            var existingNodes = await dbContext.RoadmapNodes
                .AsNoTracking()
                .Where(n => n.RoadmapId == roadmapId)
                .Select(n => new { n.Id, n.RoadmapId, n.Title, n.Description, n.Difficulty, n.OrderNo })
                .ToListAsync(cancellationToken);

            var existingNodeIds = existingNodes.Select(n => n.Id).ToHashSet();

            var existingContents = await dbContext.NodeContents
                .AsNoTracking()
                .Where(c => existingNodeIds.Contains(c.NodeId))
                .Select(c => new { c.Id, c.NodeId, c.ContentType, c.Title, c.Url, c.Description, c.EstimatedMinutes, c.Difficulty, c.OrderNo, c.IsRequired })
                .ToListAsync(cancellationToken);

            var existingEdges = await dbContext.RoadmapEdges
                .AsNoTracking()
                .Where(e => e.RoadmapId == roadmapId)
                .Select(e => new { e.Id, e.RoadmapId, e.FromNodeId, e.ToNodeId, e.EdgeType, e.OrderNo })
                .ToListAsync(cancellationToken);

            // 3. Validate payload
            var validationResult = ValidatePayload(request, roadmapId, existingNodeIds);
            if (!validationResult.isValid)
            {
                await transaction.RollbackAsync(cancellationToken);
                return new SyncRoadmapGraphResult
                {
                    Success = false,
                    Message = validationResult.errorMessage!,
                    Data = null
                };
            }

            // 4. Update roadmap metadata
            if (request.Roadmap != null)
            {
                if (!string.IsNullOrEmpty(request.Roadmap.Title))
                {
                    roadmap.Title = request.Roadmap.Title;
                }
                if (request.Roadmap.Description != null)
                {
                    roadmap.Description = request.Roadmap.Description;
                }
                dbContext.Roadmaps.Update(roadmap);
            }

            var nodeIdMap = new Dictionary<string, long>();
            var contentIdMap = new Dictionary<string, long>();

            // 5. Upsert nodes
            var payloadNodeIds = new HashSet<long>();
            foreach (var nodeItem in request.Nodes)
            {
                if (nodeItem.Id.HasValue)
                {
                    // Update existing node
                    var nodeId = nodeItem.Id.Value;
                    if (!existingNodeIds.Contains(nodeId))
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new SyncRoadmapGraphResult
                        {
                            Success = false,
                            Message = $"Node with ID {nodeId} does not belong to roadmap {roadmapId}.",
                            Data = null
                        };
                    }

                    var node = await dbContext.RoadmapNodes.FindAsync(new object[] { nodeId }, cancellationToken);
                    if (node != null)
                    {
                        node.Title = nodeItem.Title;
                        node.Description = nodeItem.Description;
                        node.Difficulty = nodeItem.Difficulty;
                        node.OrderNo = nodeItem.OrderNo;
                        dbContext.RoadmapNodes.Update(node);
                    }

                    payloadNodeIds.Add(nodeId);
                }
                else
                {
                    // Create new node
                    if (string.IsNullOrEmpty(nodeItem.ClientId))
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new SyncRoadmapGraphResult
                        {
                            Success = false,
                            Message = "ClientId is required for new nodes.",
                            Data = null
                        };
                    }

                    var newNode = new RoadmapNode
                    {
                        RoadmapId = roadmapId,
                        Title = nodeItem.Title,
                        Description = nodeItem.Description,
                        Difficulty = nodeItem.Difficulty,
                        OrderNo = nodeItem.OrderNo
                    };
                    dbContext.RoadmapNodes.Add(newNode);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    nodeIdMap[nodeItem.ClientId] = newNode.Id;
                    payloadNodeIds.Add(newNode.Id);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // Build full node mapping for resolution
            var fullNodeIdMap = new Dictionary<string, long>(nodeIdMap);
            foreach (var nodeItem in request.Nodes.Where(n => n.Id.HasValue && !string.IsNullOrEmpty(n.ClientId)))
            {
                fullNodeIdMap[nodeItem.ClientId!] = nodeItem.Id!.Value;
            }

            // 6. Upsert contents
            var payloadContentIds = new HashSet<long>();
            foreach (var contentItem in request.Contents)
            {
                var resolvedNodeId = ResolveNodeId(contentItem.NodeId, contentItem.NodeClientId, fullNodeIdMap);

                if (!resolvedNodeId.HasValue)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new SyncRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Cannot resolve node reference for content: {contentItem.NodeClientId ?? contentItem.NodeId?.ToString()}",
                        Data = null
                    };
                }

                if (!payloadNodeIds.Contains(resolvedNodeId.Value))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new SyncRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Content references node {resolvedNodeId.Value} which is not in the payload.",
                        Data = null
                    };
                }

                if (contentItem.Id.HasValue)
                {
                    // Update existing content
                    var contentId = contentItem.Id.Value;
                    var content = await dbContext.NodeContents.FindAsync(new object[] { contentId }, cancellationToken);
                    if (content == null)
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new SyncRoadmapGraphResult
                        {
                            Success = false,
                            Message = $"Content with ID {contentId} not found.",
                            Data = null
                        };
                    }

                    if (!existingNodeIds.Contains(content.NodeId))
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new SyncRoadmapGraphResult
                        {
                            Success = false,
                            Message = $"Content with ID {contentId} does not belong to roadmap {roadmapId}.",
                            Data = null
                        };
                    }

                    content.NodeId = resolvedNodeId.Value;
                    content.ContentType = contentItem.ContentType;
                    content.Title = contentItem.Title;
                    content.Url = contentItem.Url;
                    content.Description = contentItem.Description;
                    content.EstimatedMinutes = contentItem.EstimatedMinutes;
                    content.Difficulty = contentItem.Difficulty;
                    content.OrderNo = contentItem.OrderNo;
                    content.IsRequired = contentItem.IsRequired;
                    dbContext.NodeContents.Update(content);

                    payloadContentIds.Add(contentId);
                }
                else
                {
                    // Create new content
                    if (string.IsNullOrEmpty(contentItem.ClientId))
                    {
                        await transaction.RollbackAsync(cancellationToken);
                        return new SyncRoadmapGraphResult
                        {
                            Success = false,
                            Message = "ClientId is required for new contents.",
                            Data = null
                        };
                    }

                    var newContent = new NodeContent
                    {
                        NodeId = resolvedNodeId.Value,
                        ContentType = contentItem.ContentType,
                        Title = contentItem.Title,
                        Url = contentItem.Url,
                        Description = contentItem.Description,
                        EstimatedMinutes = contentItem.EstimatedMinutes,
                        Difficulty = contentItem.Difficulty,
                        OrderNo = contentItem.OrderNo,
                        IsRequired = contentItem.IsRequired
                    };
                    dbContext.NodeContents.Add(newContent);
                    await dbContext.SaveChangesAsync(cancellationToken);

                    contentIdMap[contentItem.ClientId] = newContent.Id;
                    payloadContentIds.Add(newContent.Id);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // 7. Upsert edges using identity key (FromNodeId, ToNodeId, EdgeType)
            var payloadEdgeKeys = new HashSet<(long, long, EdgeType)>();
            var existingEdgesByKey = existingEdges.ToDictionary(
                e => (e.FromNodeId, e.ToNodeId, e.EdgeType),
                e => e.Id
            );

            foreach (var edgeItem in request.Edges)
            {
                var fromNodeId = ResolveNodeId(edgeItem.FromNodeId, edgeItem.FromNodeClientId, fullNodeIdMap);
                var toNodeId = ResolveNodeId(edgeItem.ToNodeId, edgeItem.ToNodeClientId, fullNodeIdMap);

                if (!fromNodeId.HasValue || !toNodeId.HasValue)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new SyncRoadmapGraphResult
                    {
                        Success = false,
                        Message = "Cannot resolve edge node references.",
                        Data = null
                    };
                }

                if (!payloadNodeIds.Contains(fromNodeId.Value))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new SyncRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Edge references fromNode {fromNodeId.Value} which is not in the payload.",
                        Data = null
                    };
                }
                if (!payloadNodeIds.Contains(toNodeId.Value))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new SyncRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Edge references toNode {toNodeId.Value} which is not in the payload.",
                        Data = null
                    };
                }

                if (fromNodeId.Value == toNodeId.Value)
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new SyncRoadmapGraphResult
                    {
                        Success = false,
                        Message = "Self-loop edges are not allowed.",
                        Data = null
                    };
                }

                var edgeKey = (fromNodeId.Value, toNodeId.Value, edgeItem.EdgeType);
                payloadEdgeKeys.Add(edgeKey);

                if (existingEdgesByKey.TryGetValue(edgeKey, out var existingEdgeId))
                {
                    // Update existing edge
                    var edge = await dbContext.RoadmapEdges.FindAsync(new object[] { existingEdgeId }, cancellationToken);
                    if (edge != null)
                    {
                        edge.OrderNo = edgeItem.OrderNo;
                        dbContext.RoadmapEdges.Update(edge);
                    }
                }
                else
                {
                    // Create new edge
                    var newEdge = new RoadmapEdge
                    {
                        RoadmapId = roadmapId,
                        FromNodeId = fromNodeId.Value,
                        ToNodeId = toNodeId.Value,
                        EdgeType = edgeItem.EdgeType,
                        OrderNo = edgeItem.OrderNo
                    };
                    dbContext.RoadmapEdges.Add(newEdge);
                }
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // 8. Delete orphaned edges
            var edgesToDelete = existingEdges
                .Where(e => !payloadEdgeKeys.Contains((e.FromNodeId, e.ToNodeId, e.EdgeType)))
                .Select(e => e.Id)
                .ToList();

            if (edgesToDelete.Any())
            {
                await dbContext.RoadmapEdges
                    .Where(e => edgesToDelete.Contains(e.Id))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            // 9. Delete orphaned contents
            var contentsToDelete = existingContents
                .Where(c => !payloadContentIds.Contains(c.Id))
                .Select(c => c.Id)
                .ToList();

            if (contentsToDelete.Any())
            {
                await dbContext.NodeContents
                    .Where(c => contentsToDelete.Contains(c.Id))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            // 10. Delete orphaned nodes (and their remaining contents/edges via cascade)
            var nodesToDelete = existingNodeIds
                .Where(id => !payloadNodeIds.Contains(id))
                .ToList();

            if (nodesToDelete.Any())
            {
                // Delete edges referencing these nodes first
                await dbContext.RoadmapEdges
                    .Where(e => nodesToDelete.Contains(e.FromNodeId) || nodesToDelete.Contains(e.ToNodeId))
                    .ExecuteDeleteAsync(cancellationToken);

                // Delete nodes
                await dbContext.RoadmapNodes
                    .Where(n => nodesToDelete.Contains(n.Id))
                    .ExecuteDeleteAsync(cancellationToken);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            // Count final state
            var finalNodesCount = await dbContext.RoadmapNodes.CountAsync(n => n.RoadmapId == roadmapId, cancellationToken);
            var finalEdgesCount = await dbContext.RoadmapEdges.CountAsync(e => e.RoadmapId == roadmapId, cancellationToken);
            var finalContentsCount = await dbContext.NodeContents
                .Where(c => dbContext.RoadmapNodes.Any(n => n.Id == c.NodeId && n.RoadmapId == roadmapId))
                .CountAsync(cancellationToken);

            return new SyncRoadmapGraphResult
            {
                Success = true,
                Message = "Roadmap graph synchronized successfully.",
                Data = new RoadmapGraphUpdateResponse
                {
                    RoadmapId = roadmapId,
                    NodeIdMap = nodeIdMap,
                    ContentIdMap = contentIdMap,
                    Summary = new GraphSummary
                    {
                        NodesCount = finalNodesCount,
                        EdgesCount = finalEdgesCount,
                        ContentsCount = finalContentsCount
                    }
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new SyncRoadmapGraphResult
            {
                Success = false,
                Message = $"Error synchronizing roadmap graph: {ex.Message}",
                Data = null
            };
        }
    }

    private (bool isValid, string? errorMessage) ValidatePayload(SyncRoadmapGraphCommand request, long roadmapId, HashSet<long> existingNodeIds)
    {
        // Validate node clientIds are unique
        var nodeClientIds = request.Nodes.Where(n => !string.IsNullOrEmpty(n.ClientId)).Select(n => n.ClientId!).ToList();
        if (nodeClientIds.Count != nodeClientIds.Distinct().Count())
        {
            return (false, "Duplicate node clientIds found in payload.");
        }

        // Validate content clientIds are unique
        var contentClientIds = request.Contents.Where(c => !string.IsNullOrEmpty(c.ClientId)).Select(c => c.ClientId!).ToList();
        if (contentClientIds.Count != contentClientIds.Distinct().Count())
        {
            return (false, "Duplicate content clientIds found in payload.");
        }

        // Validate all existing node IDs belong to this roadmap
        var payloadExistingNodeIds = request.Nodes.Where(n => n.Id.HasValue).Select(n => n.Id!.Value).ToList();
        var invalidNodeIds = payloadExistingNodeIds.Except(existingNodeIds).ToList();
        if (invalidNodeIds.Any())
        {
            return (false, $"Nodes with IDs {string.Join(", ", invalidNodeIds)} do not belong to roadmap {roadmapId}.");
        }

        return (true, null);
    }

    private long? ResolveNodeId(long? nodeId, string? nodeClientId, Dictionary<string, long> nodeIdMap)
    {
        if (nodeId.HasValue)
        {
            return nodeId.Value;
        }

        if (!string.IsNullOrEmpty(nodeClientId) && nodeIdMap.TryGetValue(nodeClientId, out var resolvedId))
        {
            return resolvedId;
        }

        return null;
    }
}
