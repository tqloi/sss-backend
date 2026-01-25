using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.Roadmap.GetById
{
    public sealed class GetRoadmapByIdHandler(IAppDbContext dbContext) 
        : IRequestHandler<GetRoadmapByIdQuery, GetRoadmapByIdResult>
    {
        public async Task<GetRoadmapByIdResult> Handle(GetRoadmapByIdQuery request, CancellationToken cancellationToken)
        {
            var roadmap = await dbContext.Roadmaps
                .Include(r => r.Nodes)
                .Include(r => r.Edges)
                .AsNoTracking()
                .FirstOrDefaultAsync(r => r.Id == request.RoadmapId, cancellationToken);

            if (roadmap is null)
            {
                return new GetRoadmapByIdResult
                {
                    Success = false,
                    Message = "Roadmap not found.",
                    Data = null
                };
            }

            var graphDto = new RoadmapGraphDTO
            {
                Roadmap = new RoadmapBasicDTO
                {
                    Id = roadmap.Id,
                    SubjectId = roadmap.SubjectId,
                    Title = roadmap.Title,
                    Description = roadmap.Description
                },
                Nodes = roadmap.Nodes.Select(n => new RoadmapNodeDTO
                {
                    Id = n.Id,
                    RoadmapId = n.RoadmapId,
                    Title = n.Title,
                    Description = n.Description,
                    Difficulty = n.Difficulty,
                    OrderNo = n.OrderNo
                }).ToList(),
                Edges = roadmap.Edges.Select(e => new RoadmapEdgeDTO
                {
                    Id = e.Id,
                    RoadmapId = e.RoadmapId,
                    FromNodeId = e.FromNodeId,
                    ToNodeId = e.ToNodeId,
                    EdgeType = e.EdgeType,
                    OrderNo = e.OrderNo
                }).ToList()
            };

            return new GetRoadmapByIdResult
            {
                Success = true,
                Message = "Roadmap retrieved successfully.",
                Data = graphDto
            };
        }
    }
}
