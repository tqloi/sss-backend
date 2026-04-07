using MediatR;

namespace SSS.Application.Features.Content.Roadmap.Update
{
    public sealed class UpdateRoadmapCommand : IRequest<UpdateRoadmapResult>
    {
        public long Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
