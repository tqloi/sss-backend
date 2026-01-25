using MediatR;

namespace SSS.Application.Features.Content.LearningSubject.Delete
{
    public sealed class DeleteLearningSubjectCommand : IRequest<DeleteLearningSubjectResult>
    {
        public long Id { get; set; }
    }
}
