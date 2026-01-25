using SSS.Application.Common.Dtos;
using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.LearningSubject.GetAll
{
    public sealed record GetAllLearningSubjectResult(
        PaginatedResponse<LearningSubjectDTO> Subjects
    );
}
