namespace SSS.Application.Features.Content.LearningSubject.Common
{
    public sealed class LearningSubjectDTO
    {
        public long? Id { get; set; }
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
