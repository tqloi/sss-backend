using MediatR;

namespace SSS.Application.Features.Content.LearningSubject.Create
{
    public sealed class CreateLearningSubjectCommand : IRequest<CreateLearningSubjectResult>
    {
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
