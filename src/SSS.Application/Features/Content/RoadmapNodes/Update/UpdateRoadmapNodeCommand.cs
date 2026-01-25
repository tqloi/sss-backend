using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapNodes.Update
{
    public sealed class UpdateRoadmapNodeCommand : IRequest<UpdateRoadmapNodeResult>
    {
        public long RoadmapId { get; set; }
        public long NodeId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public NodeDifficulty? Difficulty { get; set; }
        public int? OrderNo { get; set; }
    }
}
