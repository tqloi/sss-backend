using SSS.Application.Features.Content.Roadmap.Common;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapNodes.GetPreviousNodeContents
{
    public sealed class PreviousRoadmapNodeContentsDto
    {
        public long Id { get; set; }
        public long RoadmapId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public NodeDifficulty? Difficulty { get; set; }
        public int? OrderNo { get; set; }
        public List<NodeContentDTO> Contents { get; set; } = new();
    }
}
