using MediatR;

namespace SSS.Application.Features.Content.LearningSubject.Update
{
    public sealed class UpdateLearningSubjectCommand : IRequest<UpdateLearningSubjectResult>
    {
        public long Id { get; set; }
        public long CategoryId { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
