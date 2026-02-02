using SSS.Domain.Enums;

namespace SSS.Application.Features.StudyPlans.StudyPlans.Common
{
    public class StudyPlanDto
    {
        public long Id { get; set; }
        public string UserId { get; set; } = null!;
        public long RoadmapId { get; set; }
        public string RoadmapName { get; set; } = null!;
        public StudyPlanStrategy? Strategy { get; set; }
        public StudyPlanStatus? Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public IEnumerable<StudyModuleDto> Modules { get; set; } = new List<StudyModuleDto>();
    }

    public class StudyModuleDto
    {
        public long Id { get; set; }
        public long StudyPlanId { get; set; }
        public long RoadmapNodeId { get; set; }
        public string RoadmapNodeName { get; set; } = null!;
        public ModuleStatus? Status { get; set; }
    }
}