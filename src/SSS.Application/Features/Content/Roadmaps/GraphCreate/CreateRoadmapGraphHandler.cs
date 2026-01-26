using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmaps.Common;


namespace SSS.Application.Features.Content.Roadmaps.GraphCreate;

using SSS.Domain.Entities.Content;
public sealed class CreateRoadmapGraphHandler(IAppDbContext dbContext) 
    : IRequestHandler<CreateRoadmapGraphCommand, CreateRoadmapGraphResult>
{
    public async Task<CreateRoadmapGraphResult> Handle(CreateRoadmapGraphCommand request, CancellationToken cancellationToken)
    {
        // Validate subject exists
        var subjectExists = await dbContext.LearningSubjects
            .AnyAsync(s => s.Id == request.Roadmap.SubjectId, cancellationToken);
        
        if (!subjectExists)
        {
            return new CreateRoadmapGraphResult
            {
                Success = false,
                Message = $"Subject with ID {request.Roadmap.SubjectId} not found.",
                Data = null
            };
        }

        // Validate node clientIds are unique
        var nodeClientIds = request.Nodes.Where(n => !string.IsNullOrEmpty(n.ClientId)).Select(n => n.ClientId!).ToList();
        if (nodeClientIds.Count != nodeClientIds.Distinct().Count())
        {
            return new CreateRoadmapGraphResult
            {
                Success = false,
                Message = "Duplicate node clientIds found in payload.",
                Data = null
            };
        }

        // Validate content clientIds are unique
        var contentClientIds = request.Contents.Where(c => !string.IsNullOrEmpty(c.ClientId)).Select(c => c.ClientId!).ToList();
        if (contentClientIds.Count != contentClientIds.Distinct().Count())
        {
            return new CreateRoadmapGraphResult
            {
                Success = false,
                Message = "Duplicate content clientIds found in payload.",
                Data = null
            };
        }

        // Validate content (node, orderNo) uniqueness
        var contentNodeOrderPairs = new HashSet<(string, int)>();
        foreach (var content in request.Contents)
        {
            var nodeRef = content.NodeClientId ?? content.NodeId?.ToString();
            if (string.IsNullOrEmpty(nodeRef))
            {
                return new CreateRoadmapGraphResult
                {
                    Success = false,
                    Message = "Content must reference a node via nodeId or nodeClientId.",
                    Data = null
                };
            }

            var pair = (nodeRef, content.OrderNo);
            if (!contentNodeOrderPairs.Add(pair))
            {
                return new CreateRoadmapGraphResult
                {
                    Success = false,
                    Message = $"Duplicate (node, orderNo) found for content: {nodeRef}, {content.OrderNo}",
                    Data = null
                };
            }
        }

        // Validate edges: no self-loops, no duplicates
        var edgeKeys = new HashSet<(string, string, string)>();
        foreach (var edge in request.Edges)
        {
            var fromRef = edge.FromNodeClientId ?? edge.FromNodeId?.ToString();
            var toRef = edge.ToNodeClientId ?? edge.ToNodeId?.ToString();

            if (string.IsNullOrEmpty(fromRef))
            {
                return new CreateRoadmapGraphResult
                {
                    Success = false,
                    Message = "Edge must reference fromNode.",
                    Data = null
                };
            }

            if (string.IsNullOrEmpty(toRef))
            {
                return new CreateRoadmapGraphResult
                {
                    Success = false,
                    Message = "Edge must reference toNode.",
                    Data = null
                };
            }
            
            if (fromRef == toRef)
            {
                return new CreateRoadmapGraphResult
                {
                    Success = false,
                    Message = "Self-loop edges are not allowed.",
                    Data = null
                };
            }

            var key = (fromRef, toRef, edge.EdgeType.ToString());
            if (!edgeKeys.Add(key))
            {
                return new CreateRoadmapGraphResult
                {
                    Success = false,
                    Message = $"Duplicate edge found: {fromRef} -> {toRef} ({edge.EdgeType})",
                    Data = null
                };
            }
        }

        // Start transaction
        await using var transaction = await dbContext.BeginTransactionAsync(cancellationToken);

        try
        {
            // 1. Create roadmap
            var roadmap = new Roadmap
            {
                SubjectId = request.Roadmap.SubjectId,
                Title = request.Roadmap.Title,
                Description = request.Roadmap.Description
            };

            dbContext.Roadmaps.Add(roadmap);
            await dbContext.SaveChangesAsync(cancellationToken);

            var nodeIdMap = new Dictionary<string, long>();
            var contentIdMap = new Dictionary<string, long>();

            // 2. Create nodes
            var nodesToCreate = new List<RoadmapNode>();
            foreach (var nodeItem in request.Nodes)
            {
                var node = new RoadmapNode
                {
                    RoadmapId = roadmap.Id,
                    Title = nodeItem.Title,
                    Description = nodeItem.Description,
                    Difficulty = nodeItem.Difficulty,
                    OrderNo = nodeItem.OrderNo
                };
                nodesToCreate.Add(node);
                dbContext.RoadmapNodes.Add(node);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // Map clientIds to dbIds
            for (int i = 0; i < request.Nodes.Count; i++)
            {
                var clientId = request.Nodes[i].ClientId;
                if (!string.IsNullOrEmpty(clientId))
                {
                    nodeIdMap[clientId] = nodesToCreate[i].Id;
                }
            }

            // 3. Create contents
            var contentsToCreate = new List<NodeContent>();
            foreach (var contentItem in request.Contents)
            {
                var resolvedNodeId = ResolveNodeId(contentItem.NodeId, contentItem.NodeClientId, nodeIdMap);
                if (!resolvedNodeId.HasValue || !nodeIdMap.ContainsValue(resolvedNodeId.Value))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new CreateRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Content references non-existent node: {contentItem.NodeClientId ?? contentItem.NodeId?.ToString()}",
                        Data = null
                    };
                }

                var content = new NodeContent
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
                contentsToCreate.Add(content);
                dbContext.NodeContents.Add(content);
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            // Map clientIds to dbIds for contents
            for (int i = 0; i < request.Contents.Count; i++)
            {
                var clientId = request.Contents[i].ClientId;
                if (!string.IsNullOrEmpty(clientId))
                {
                    contentIdMap[clientId] = contentsToCreate[i].Id;
                }
            }

            // 4. Create edges
            var edgesToCreate = new List<RoadmapEdge>();
            foreach (var edgeItem in request.Edges)
            {
                var resolvedFromNodeId = ResolveNodeId(edgeItem.FromNodeId, edgeItem.FromNodeClientId, nodeIdMap);
                var resolvedToNodeId = ResolveNodeId(edgeItem.ToNodeId, edgeItem.ToNodeClientId, nodeIdMap);

                if (!resolvedFromNodeId.HasValue || !nodeIdMap.ContainsValue(resolvedFromNodeId.Value))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new CreateRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Edge references non-existent fromNode: {edgeItem.FromNodeClientId ?? edgeItem.FromNodeId?.ToString()}",
                        Data = null
                    };
                }
                if (!resolvedToNodeId.HasValue || !nodeIdMap.ContainsValue(resolvedToNodeId.Value))
                {
                    await transaction.RollbackAsync(cancellationToken);
                    return new CreateRoadmapGraphResult
                    {
                        Success = false,
                        Message = $"Edge references non-existent toNode: {edgeItem.ToNodeClientId ?? edgeItem.ToNodeId?.ToString()}",
                        Data = null
                    };
                }

                var edge = new RoadmapEdge
                {
                    RoadmapId = roadmap.Id,
                    FromNodeId = resolvedFromNodeId.Value,
                    ToNodeId = resolvedToNodeId.Value,
                    EdgeType = edgeItem.EdgeType,
                    OrderNo = edgeItem.OrderNo
                };
                edgesToCreate.Add(edge);
                dbContext.RoadmapEdges.Add(edge);
            }

            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);

            return new CreateRoadmapGraphResult
            {
                Success = true,
                Message = "Roadmap graph created successfully.",
                Data = new RoadmapGraphCreateResponse
                {
                    RoadmapId = roadmap.Id,
                    NodeIdMap = nodeIdMap,
                    ContentIdMap = contentIdMap,
                    Summary = new GraphSummary
                    {
                        NodesCount = nodesToCreate.Count,
                        EdgesCount = edgesToCreate.Count,
                        ContentsCount = contentsToCreate.Count
                    }
                }
            };
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return new CreateRoadmapGraphResult
            {
                Success = false,
                Message = $"Error creating roadmap graph: {ex.Message}",
                Data = null
            };
        }
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
    