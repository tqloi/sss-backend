using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.NodeContents.Update
{
    public sealed class UpdateNodeContentHandler(IAppDbContext dbContext) 
        : IRequestHandler<UpdateNodeContentCommand, UpdateNodeContentResult>
    {
        public async Task<UpdateNodeContentResult> Handle(UpdateNodeContentCommand request, CancellationToken cancellationToken)
        {
            // Validate node belongs to roadmap
            var nodeExists = await dbContext.RoadmapNodes
                .AnyAsync(n => n.Id == request.NodeId && n.RoadmapId == request.RoadmapId, cancellationToken);

            if (!nodeExists)
            {
                return new UpdateNodeContentResult
                {
                    Success = false,
                    Message = "Node not found or does not belong to this roadmap.",
                    Data = null
                };
            }

            var entity = await dbContext.NodeContents
                .FirstOrDefaultAsync(c => c.Id == request.ContentId && c.NodeId == request.NodeId, cancellationToken);

            if (entity is null)
            {
                return new UpdateNodeContentResult
                {
                    Success = false,
                    Message = "Content not found or does not belong to this node.",
                    Data = null
                };
            }

            // Partial update
            if (request.ContentType.HasValue)
            {
                entity.ContentType = request.ContentType.Value;
            }

            if (request.Title is not null)
            {
                entity.Title = request.Title;
            }

            if (request.Url is not null)
            {
                entity.Url = request.Url;
            }

            if (request.Description is not null)
            {
                entity.Description = request.Description;
            }

            if (request.EstimatedMinutes.HasValue)
            {
                entity.EstimatedMinutes = request.EstimatedMinutes;
            }

            if (request.Difficulty is not null)
            {
                entity.Difficulty = request.Difficulty;
            }

            if (request.OrderNo.HasValue)
            {
                entity.OrderNo = request.OrderNo.Value;
            }

            if (request.IsRequired.HasValue)
            {
                entity.IsRequired = request.IsRequired.Value;
            }

            await dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateNodeContentResult
            {
                Success = true,
                Message = "Node content updated successfully.",
                Data = new NodeContentDTO
                {
                    Id = entity.Id,
                    NodeId = entity.NodeId,
                    ContentType = entity.ContentType,
                    Title = entity.Title,
                    Url = entity.Url,
                    Description = entity.Description,
                    EstimatedMinutes = entity.EstimatedMinutes,
                    Difficulty = entity.Difficulty,
                    OrderNo = entity.OrderNo,
                    IsRequired = entity.IsRequired
                }
            };
        }
    }
}
