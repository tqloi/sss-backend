using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapNodes.GetPreviousNodeContents
{
    public sealed class GetPreviousNodeContentsHandler(IAppDbContext dbContext)
        : IRequestHandler<GetPreviousNodeContentsQuery, GetPreviousNodeContentsResult>
    {
        public async Task<GetPreviousNodeContentsResult> Handle(GetPreviousNodeContentsQuery request, CancellationToken cancellationToken)
        {
            var plan = await dbContext.StudyPlans
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == request.StudyPlanId, cancellationToken);

            if (plan is null)
            {
                return new GetPreviousNodeContentsResult
                {
                    Success = false,
                    Message = "Study plan not found.",
                    Data = null
                };
            }

            var currentNodeInPlan = await dbContext.StudyPlanModules
                .AsNoTracking()
                .AnyAsync(m => m.StudyPlanId == request.StudyPlanId && m.RoadmapNodeId == request.RoadmapNodeId, cancellationToken);

            if (!currentNodeInPlan)
            {
                return new GetPreviousNodeContentsResult
                {
                    Success = false,
                    Message = "Roadmap node does not belong to this study plan.",
                    Data = null
                };
            }

            var studyPlanNodeIds = dbContext.StudyPlanModules
                .AsNoTracking()
                .Where(m => m.StudyPlanId == request.StudyPlanId)
                .Select(m => m.RoadmapNodeId);

            long? previousNodeId = await dbContext.RoadmapEdges
                .AsNoTracking()
                .Where(e => e.RoadmapId == plan.RoadmapId
                            && e.ToNodeId == request.RoadmapNodeId
                            && studyPlanNodeIds.Contains(e.FromNodeId))
                .OrderBy(e => e.EdgeType == EdgeType.Next
                    ? 0
                    : e.EdgeType == EdgeType.Recommended
                        ? 1
                        : 2)
                .ThenBy(e => e.OrderNo ?? int.MaxValue)
                .ThenBy(e => e.Id)
                .Select(e => (long?)e.FromNodeId)
                .FirstOrDefaultAsync(cancellationToken);

            if (previousNodeId is null)
            {
                var currentOrderNo = await dbContext.RoadmapNodes
                    .AsNoTracking()
                    .Where(n => n.Id == request.RoadmapNodeId && n.RoadmapId == plan.RoadmapId)
                    .Select(n => n.OrderNo)
                    .FirstOrDefaultAsync(cancellationToken);

                if (currentOrderNo.HasValue)
                {
                    previousNodeId = await dbContext.RoadmapNodes
                        .AsNoTracking()
                        .Where(n => n.RoadmapId == plan.RoadmapId
                                    && n.OrderNo.HasValue
                                    && n.OrderNo < currentOrderNo.Value
                                    && studyPlanNodeIds.Contains(n.Id))
                        .OrderByDescending(n => n.OrderNo)
                        .ThenByDescending(n => n.Id)
                        .Select(n => (long?)n.Id)
                        .FirstOrDefaultAsync(cancellationToken);
                }
            }

            if (previousNodeId is null)
            {
                return new GetPreviousNodeContentsResult
                {
                    Success = true,
                    Message = "No previous node found.",
                    Data = new List<NodeContentDTO>()
                };
            }

            var contents = await dbContext.NodeContents
                .AsNoTracking()
                .Where(c => c.NodeId == previousNodeId.Value)
                .OrderBy(c => c.OrderNo)
                .ToListAsync(cancellationToken);

            var result = contents.Select(c => new NodeContentDTO
            {
                Id = c.Id,
                NodeId = c.NodeId,
                ContentType = c.ContentType,
                Title = c.Title,
                Url = c.Url,
                Description = c.Description,
                EstimatedMinutes = c.EstimatedMinutes,
                Difficulty = c.Difficulty,
                OrderNo = c.OrderNo,
                IsRequired = c.IsRequired
            }).ToList();

            return new GetPreviousNodeContentsResult
            {
                Success = true,
                Message = "Previous node contents retrieved successfully.",
                Data = result
            };
        }
    }
}
