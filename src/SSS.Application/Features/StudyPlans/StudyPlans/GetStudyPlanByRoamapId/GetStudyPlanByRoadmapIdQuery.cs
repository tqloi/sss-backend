using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudyPlans.StudyPlans.GetStudyPlanByRoamapId
{
    public class GetStudyPlanByRoadmapIdQuery : IRequest<GetStudyPlanByRoadmapIdResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public long RoadmapId { get; set; }
    }
}
