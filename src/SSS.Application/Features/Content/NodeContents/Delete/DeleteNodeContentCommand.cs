using MediatR;

namespace SSS.Application.Features.Content.NodeContents.Delete
{
    public sealed record DeleteNodeContentCommand(long RoadmapId, long NodeId, long ContentId) : IRequest<DeleteNodeContentResult>;
}
