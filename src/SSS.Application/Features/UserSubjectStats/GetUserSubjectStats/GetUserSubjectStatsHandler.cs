using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.GetUserSubjectStats;

public sealed class GetUserSubjectStatsHandler(
    IAppDbContext dbContext
) : IRequestHandler<GetUserSubjectStatsQuery, List<UserSubjectStatDto>>
{
    public async Task<List<UserSubjectStatDto>> Handle(GetUserSubjectStatsQuery request, CancellationToken ct)
    {
        var stats = await dbContext.UserSubjectStats
            .Include(s => s.Subject)
            .Where(s => s.UserId == request.UserId)
            .OrderByDescending(s => s.UpdatedAt)
            .Select(s => new UserSubjectStatDto
            {
                Id = s.Id,
                UserId = s.UserId,
                SubjectId = s.SubjectId,
                SubjectName = s.Subject.Name,
                ProficiencyLevel = s.ProficiencyLevel,
                TotalHoursSpent = s.TotalHoursSpent,
                WeakNodeIds = s.WeakNodeIds,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync(ct);

        return stats;
    }
}
