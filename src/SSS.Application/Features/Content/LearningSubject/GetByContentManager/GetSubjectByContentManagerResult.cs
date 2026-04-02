using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.LearningSubject.GetByContentManager
{
    public sealed record GetSubjectByContentManagerResult(List<LearningSubjectDTO> Subjects);
}
