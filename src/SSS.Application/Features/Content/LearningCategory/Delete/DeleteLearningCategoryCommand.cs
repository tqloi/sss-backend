using MediatR;

namespace SSS.Application.Features.Content.LearningCategory.Delete
{
    public sealed class DeleteLearningCategoryCommand : IRequest<DeleteLearningCategoryResult>
    {
        public long Id { get; set; }
    }
}
