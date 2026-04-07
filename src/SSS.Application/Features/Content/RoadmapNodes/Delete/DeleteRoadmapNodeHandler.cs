using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.RoadmapNodes.Delete
{
    public sealed class DeleteRoadmapNodeHandler(IAppDbContext dbContext) 
        : IRequestHandler<DeleteRoadmapNodeCommand, DeleteRoadmapNodeResult>
    {
        public async Task<DeleteRoadmapNodeResult> Handle(DeleteRoadmapNodeCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.RoadmapNodes
                .Include(n => n.Contents)
                .Include(n => n.OutgoingEdges)
                .Include(n => n.IncomingEdges)
                .FirstOrDefaultAsync(x => x.Id == request.NodeId && x.RoadmapId == request.RoadmapId, cancellationToken);

            if (entity is null)
            {
                return new DeleteRoadmapNodeResult(false, "Roadmap node not found or does not belong to this roadmap.");
            }

            // Hard delete: Remove all related edges referencing this node
            // Due to DeleteBehavior.Restrict on edges, we must manually delete them
            dbContext.RoadmapEdges.RemoveRange(entity.OutgoingEdges);
            dbContext.RoadmapEdges.RemoveRange(entity.IncomingEdges);

            // NodeContents will cascade delete via RoadmapNode deletion
            dbContext.RoadmapNodes.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteRoadmapNodeResult(true, "Roadmap node deleted successfully.");
        }
    }
}
