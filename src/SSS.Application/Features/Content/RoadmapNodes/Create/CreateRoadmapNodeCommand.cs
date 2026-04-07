using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.RoadmapNodes.Create
{
    public sealed class CreateRoadmapNodeCommand : IRequest<CreateRoadmapNodeResult>
    {
        public long RoadmapId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public NodeDifficulty? Difficulty { get; set; }
        public int? OrderNo { get; set; }
    }
}
