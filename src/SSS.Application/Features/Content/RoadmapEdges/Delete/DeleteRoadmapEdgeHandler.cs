using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.RoadmapEdges.Delete
{
    public sealed class DeleteRoadmapEdgeHandler(IAppDbContext dbContext) 
        : IRequestHandler<DeleteRoadmapEdgeCommand, DeleteRoadmapEdgeResult>
    {
        public async Task<DeleteRoadmapEdgeResult> Handle(DeleteRoadmapEdgeCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.RoadmapEdges
                .FirstOrDefaultAsync(x => x.Id == request.EdgeId && x.RoadmapId == request.RoadmapId, cancellationToken);

            if (entity is null)
            {
                return new DeleteRoadmapEdgeResult(false, "Roadmap edge not found or does not belong to this roadmap.");
            }

            dbContext.RoadmapEdges.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteRoadmapEdgeResult(true, "Roadmap edge deleted successfully.");
        }
    }
}
