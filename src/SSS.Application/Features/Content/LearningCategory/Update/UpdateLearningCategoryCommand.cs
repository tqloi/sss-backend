using MediatR;

namespace SSS.Application.Features.Content.LearningCategory.Update
{
    public sealed class UpdateLearningCategoryCommand : IRequest<UpdateLearningCategoryResult>
    {
        public long Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
