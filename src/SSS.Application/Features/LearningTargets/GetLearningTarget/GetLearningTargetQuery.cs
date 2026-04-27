using MediatR;

namespace SSS.Application.Features.LearningTargets.GetLearningTarget
{
    public class GetLearningTargetQuery : IRequest<GetLearningTargetResult?>
    {
        public string UserId { get; set; } = default!;
        public long RoadmapId { get; set; }
    }
}
