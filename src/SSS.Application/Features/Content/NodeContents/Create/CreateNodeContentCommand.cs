using MediatR;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Content.NodeContents.Create
{
    public sealed class CreateNodeContentCommand : IRequest<CreateNodeContentResult>
    {
        public long RoadmapId { get; set; }
        public long NodeId { get; set; }
        public ContentType ContentType { get; set; }
        public string Title { get; set; } = null!;
        public string? Url { get; set; }
        public string? Description { get; set; }
        public int? EstimatedMinutes { get; set; }
        public string? Difficulty { get; set; }
        public int OrderNo { get; set; }
        public bool IsRequired { get; set; }
    }
}
