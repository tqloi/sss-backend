namespace SSS.Application.Features.Content.LearningCategory.Common
{
    public sealed class LearningCategoryDTO
    {
        public long? Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
