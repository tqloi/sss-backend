using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapEdges.BulkSync
{
    public sealed class EdgeItem
    {
        public long FromNodeId { get; set; }
        public long ToNodeId { get; set; }
        public EdgeType EdgeType { get; set; }
        public int? OrderNo { get; set; }
    }

    public sealed class BulkSyncEdgesCommand : IRequest<BulkSyncEdgesResult>
    {
        public long RoadmapId { get; set; }
        public List<EdgeItem> Edges { get; set; } = new();
    }
}
