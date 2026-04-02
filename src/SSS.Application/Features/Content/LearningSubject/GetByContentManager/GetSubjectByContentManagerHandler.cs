using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.Content.LearningSubject.Common;

namespace SSS.Application.Features.Content.LearningSubject.GetByContentManager
{
    public sealed class GetSubjectByContentManagerHandler(IAppDbContext dbContext)
        : IRequestHandler<GetSubjectByContentManagerQuery, GetSubjectByContentManagerResult>
    {
        public async Task<GetSubjectByContentManagerResult> Handle(
            GetSubjectByContentManagerQuery request,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.ManagerId))
            {
                throw new UnauthorizedAccessException("Manager id is required.");
            }

            var subjectIds = await dbContext.ContentManagerSubjects
                .AsNoTracking()
                .Where(x => x.ContentManagerId == request.ManagerId && x.IsActive)
                .Select(x => x.SubjectId)
                .Distinct()
                .ToListAsync(cancellationToken);

            if (subjectIds.Count == 0)
            {
                return new GetSubjectByContentManagerResult(new List<LearningSubjectDTO>());
            }

            var subjects = await dbContext.LearningSubjects
                .AsNoTracking()
                .Where(s => subjectIds.Contains(s.Id) && s.IsActive)
                .OrderBy(s => s.Name)
                .Select(subject => new LearningSubjectDTO
                {
                    Id = subject.Id,
                    CategoryId = subject.CategoryId,
                    Name = subject.Name,
                    Description = subject.Description,
                    IsActive = subject.IsActive
                })
                .ToListAsync(cancellationToken);

            return new GetSubjectByContentManagerResult(subjects);
        }
    }
}
