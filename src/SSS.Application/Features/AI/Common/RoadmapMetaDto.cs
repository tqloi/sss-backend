namespace SSS.Application.Features.AI.Common
{
    public sealed class RoadmapMetaDto
    {
        public long SubjectId { get; init; }
        public string Title { get; init; } = null!;
        public string? Description { get; init; }
    }
}