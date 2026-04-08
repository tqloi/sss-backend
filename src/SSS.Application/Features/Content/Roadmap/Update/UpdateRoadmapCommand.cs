using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.Roadmap.Update
{
    public sealed class UpdateRoadmapCommand : IRequest<UpdateRoadmapResult>
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public RoadmapStatus? Status { get; set; }
    }
}
