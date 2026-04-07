using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.StudyPlans.StudyPlans.CreateStudyPlan
{
    public class CreateStudyPlanCommand : IRequest<CreateStudyPlanResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public long RoadmapId { get; set; }
    }
}
