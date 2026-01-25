using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.NodeContents.Delete
{
    public sealed class DeleteNodeContentHandler(IAppDbContext dbContext) 
        : IRequestHandler<DeleteNodeContentCommand, DeleteNodeContentResult>
    {
        public async Task<DeleteNodeContentResult> Handle(DeleteNodeContentCommand request, CancellationToken cancellationToken)
        {
            // Validate node belongs to roadmap
            var nodeExists = await dbContext.RoadmapNodes
                .AnyAsync(n => n.Id == request.NodeId && n.RoadmapId == request.RoadmapId, cancellationToken);

            if (!nodeExists)
            {
                return new DeleteNodeContentResult(false, "Node not found or does not belong to this roadmap.");
            }

            var entity = await dbContext.NodeContents
                .FirstOrDefaultAsync(c => c.Id == request.ContentId && c.NodeId == request.NodeId, cancellationToken);

            if (entity is null)
            {
                return new DeleteNodeContentResult(false, "Content not found or does not belong to this node.");
            }

            dbContext.NodeContents.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteNodeContentResult(true, "Node content deleted successfully.");
        }
    }
}
