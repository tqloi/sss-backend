using MediatR;

namespace SSS.Application.Features.Content.LearningCategory.Create
{
    public sealed class CreateLearningCategoryCommand : IRequest<CreateLearningCategoryResult>
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}