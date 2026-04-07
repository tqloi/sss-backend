using MediatR;

namespace SSS.Application.Features.Content.NodeContents.GetAll
{
    public sealed record GetNodeContentsQuery(long RoadmapId, long NodeId) : IRequest<GetNodeContentsResult>;
}
