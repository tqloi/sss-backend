using MediatR;

namespace SSS.Application.Features.Content.LearningSubject.GetAll
{
    public sealed record GetAllLearningSubjectQuery(
        int PageIndex,
        int PageSize,
        long? CategoryId = null
    ) : IRequest<GetAllLearningSubjectResult>;
}
