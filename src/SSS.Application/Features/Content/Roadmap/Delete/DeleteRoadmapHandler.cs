using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Content.Roadmap.Delete
{
    public sealed class DeleteRoadmapHandler(IAppDbContext dbContext) 
        : IRequestHandler<DeleteRoadmapCommand, DeleteRoadmapResult>
    {
        public async Task<DeleteRoadmapResult> Handle(DeleteRoadmapCommand request, CancellationToken cancellationToken)
        {
            var entity = await dbContext.Roadmaps
                .Include(r => r.Nodes)
                    .ThenInclude(n => n.Contents)
                .Include(r => r.Edges)
                .FirstOrDefaultAsync(x => x.Id == request.RoadmapId, cancellationToken);

            if (entity is null)
            {
                return new DeleteRoadmapResult(false, "Roadmap not found.");
            }

            // Hard delete: Remove all related data
            // NodeContents will cascade delete via Node deletion
            // Edges will cascade delete via Roadmap deletion
            // Nodes will cascade delete via Roadmap deletion (configured in EF)
            dbContext.Roadmaps.Remove(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new DeleteRoadmapResult(true, "Roadmap deleted successfully.");
        }
    }
}
