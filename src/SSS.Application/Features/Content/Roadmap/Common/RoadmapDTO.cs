using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.Roadmap.Common
{
    public class RoadmapDTO
    {
        public long Id { get; set; }
        public long SubjectId { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public LearningSubjectDTO? Subject { get; set; }
    }
}
