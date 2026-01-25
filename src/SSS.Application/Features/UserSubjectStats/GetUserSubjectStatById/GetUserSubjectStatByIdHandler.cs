using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.GetUserSubjectStatById;

public sealed class GetUserSubjectStatByIdHandler(
    IAppDbContext dbContext
) : IRequestHandler<GetUserSubjectStatByIdQuery, UserSubjectStatDto?>
{
    public async Task<UserSubjectStatDto?> Handle(GetUserSubjectStatByIdQuery request, CancellationToken ct)
    {
        var stat = await dbContext.UserSubjectStats
            .Include(s => s.Subject)
            .Where(s => s.Id == request.Id && s.UserId == request.UserId)
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
            .FirstOrDefaultAsync(ct);

        return stat;
    }
}
