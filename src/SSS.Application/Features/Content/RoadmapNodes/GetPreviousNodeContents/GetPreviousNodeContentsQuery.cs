using MediatR;

namespace SSS.Application.Features.Content.RoadmapNodes.GetPreviousNodeContents
{
    public sealed class GetPreviousNodeContentsQuery : IRequest<GetPreviousNodeContentsResult>
    {
        public long StudyPlanId { get; set; }
        public long RoadmapNodeId { get; set; }
    }
}
