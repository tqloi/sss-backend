using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.NodeContents.GetAll
{
    public sealed class GetNodeContentsHandler(IAppDbContext dbContext) 
        : IRequestHandler<GetNodeContentsQuery, GetNodeContentsResult>
    {
        public async Task<GetNodeContentsResult> Handle(GetNodeContentsQuery request, CancellationToken cancellationToken)
        {
            // Validate node exists and belongs to roadmap
            var nodeExists = await dbContext.RoadmapNodes
                .AnyAsync(n => n.Id == request.NodeId && n.RoadmapId == request.RoadmapId, cancellationToken);

            if (!nodeExists)
            {
                return new GetNodeContentsResult
                {
                    Success = false,
                    Message = "Node not found or does not belong to this roadmap.",
                    Data = null
                };
            }

            var contents = await dbContext.NodeContents
                .Where(c => c.NodeId == request.NodeId)
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

            return new GetNodeContentsResult
            {
                Success = true,
                Message = "Node contents retrieved successfully.",
                Data = result
            };
        }
    }
}
