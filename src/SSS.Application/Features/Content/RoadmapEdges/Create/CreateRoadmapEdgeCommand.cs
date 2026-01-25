using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapEdges.Create
{
    public sealed class CreateRoadmapEdgeCommand : IRequest<CreateRoadmapEdgeResult>
    {
        public long RoadmapId { get; set; }
        public long FromNodeId { get; set; }
        public long ToNodeId { get; set; }
        public EdgeType EdgeType { get; set; }
        public int? OrderNo { get; set; }
    }
}
