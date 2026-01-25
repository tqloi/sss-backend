namespace SSS.Web.Endpoints.Content.LearningCategory.Create
{
    public class CreateLearningCategoryRequest
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
