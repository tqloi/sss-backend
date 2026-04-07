using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Entities.Content;

namespace SSS.Application.Features.Content.RoadmapNodes.Create
{
    public sealed class CreateRoadmapNodeHandler(IAppDbContext dbContext) 
        : IRequestHandler<CreateRoadmapNodeCommand, CreateRoadmapNodeResult>
    {
        public async Task<CreateRoadmapNodeResult> Handle(CreateRoadmapNodeCommand request, CancellationToken cancellationToken)
        {
            // Validate RoadmapId exists
            var roadmapExists = await dbContext.Roadmaps
                .AnyAsync(x => x.Id == request.RoadmapId, cancellationToken);

            if (!roadmapExists)
            {
                return new CreateRoadmapNodeResult
                {
                    Success = false,
                    Message = "Roadmap not found.",
                    Data = null
                };
            }

            var entity = new RoadmapNode
            {
                RoadmapId = request.RoadmapId,
                Title = request.Title,
                Description = request.Description,
                Difficulty = request.Difficulty,
                OrderNo = request.OrderNo
            };

            dbContext.RoadmapNodes.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateRoadmapNodeResult
            {
                Success = true,
                Message = "Roadmap node created successfully.",
                Data = new RoadmapNodeDTO
                {
                    Id = entity.Id,
                    RoadmapId = entity.RoadmapId,
                    Title = entity.Title,
                    Description = entity.Description,
                    Difficulty = entity.Difficulty,
                    OrderNo = entity.OrderNo
                }
            };
        }
    }
}
