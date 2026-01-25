using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.Content.NodeContents.Create
{
    public sealed class CreateNodeContentHandler(IAppDbContext dbContext) 
        : IRequestHandler<CreateNodeContentCommand, CreateNodeContentResult>
    {
        public async Task<CreateNodeContentResult> Handle(CreateNodeContentCommand request, CancellationToken cancellationToken)
        {
            // Validate node exists and belongs to roadmap
            var nodeExists = await dbContext.RoadmapNodes
                .AnyAsync(n => n.Id == request.NodeId && n.RoadmapId == request.RoadmapId, cancellationToken);

            if (!nodeExists)
            {
                return new CreateNodeContentResult
                {
                    Success = false,
                    Message = "Node not found or does not belong to this roadmap.",
                    Data = null
                };
            }

            var entity = new NodeContent
            {
                NodeId = request.NodeId,
                ContentType = request.ContentType,
                Title = request.Title,
                Url = request.Url,
                Description = request.Description,
                EstimatedMinutes = request.EstimatedMinutes,
                Difficulty = request.Difficulty,
                OrderNo = request.OrderNo,
                IsRequired = request.IsRequired
            };

            dbContext.NodeContents.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateNodeContentResult
            {
                Success = true,
                Message = "Node content created successfully.",
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
