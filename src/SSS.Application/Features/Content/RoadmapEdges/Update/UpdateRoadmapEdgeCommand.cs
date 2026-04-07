using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapEdges.Update
{
    public sealed class UpdateRoadmapEdgeCommand : IRequest<UpdateRoadmapEdgeResult>
    {
        public long RoadmapId { get; set; }
        public long EdgeId { get; set; }
        public EdgeType? EdgeType { get; set; }
        public int? OrderNo { get; set; }
    }
}
