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
            var subjects = await dbContext.ContentManagerSubjects
                .AsNoTracking()
                .Where(x => x.ContentManagerId == request.ManagerId && x.IsActive)
                .Select(x => x.Subject)
                .Where(s => s.IsActive)
                .GroupBy(s => s.Id)
                .Select(g => g.First())
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
