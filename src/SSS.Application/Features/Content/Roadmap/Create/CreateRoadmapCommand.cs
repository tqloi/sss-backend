using MediatR;

namespace SSS.Application.Features.Content.Roadmap.Create
{
    public sealed class CreateRoadmapCommand : IRequest<CreateRoadmapResult>
    {
        public long SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
    }
}
