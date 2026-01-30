namespace SSS.Application.Features.AI.Common
{
    public sealed class RoadmapEdgeDto
    {
        public long FromNodeId { get; init; }
        public long ToNodeId { get; init; }
        public string EdgeType { get; init; } = null!; // Prerequisite | Optional | Parallel
        public int OrderNo { get; init; }
    }
}