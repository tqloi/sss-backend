using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.Roadmap.Common;

namespace SSS.Application.Features.Content.RoadmapEdges.BulkSync
{
    public sealed class BulkSyncEdgesResult() : GenericResponseClass<List<RoadmapEdgeDTO>>;
}
