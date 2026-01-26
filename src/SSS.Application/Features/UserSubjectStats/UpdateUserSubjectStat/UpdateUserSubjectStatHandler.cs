using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserSubjectStats.Common;

namespace SSS.Application.Features.UserSubjectStats.UpdateUserSubjectStat;

public sealed class UpdateUserSubjectStatHandler(
    IAppDbContext dbContext
) : IRequestHandler<UpdateUserSubjectStatCommand, UserSubjectStatDto>
{
    public async Task<UserSubjectStatDto> Handle(UpdateUserSubjectStatCommand request, CancellationToken ct)
    {
        var stat = await dbContext.UserSubjectStats
            .Include(s => s.Subject)
            .FirstOrDefaultAsync(s => s.Id == request.Id && s.UserId == request.UserId, ct);

        if (stat is null)
            throw new InvalidOperationException("User subject stat not found.");

        // Update fields if provided
        if (request.ProficiencyLevel.HasValue)
            stat.ProficiencyLevel = request.ProficiencyLevel;

        if (request.TotalHoursSpent.HasValue)
            stat.TotalHoursSpent = request.TotalHoursSpent;

        if (request.WeakNodeIds is not null)
            stat.WeakNodeIds = request.WeakNodeIds;

        stat.UpdatedAt = DateTime.UtcNow;

        await dbContext.SaveChangesAsync(ct);

        return new UserSubjectStatDto
        {
            Id = stat.Id,
            UserId = stat.UserId,
            SubjectId = stat.SubjectId,
            SubjectName = stat.Subject.Name,
            ProficiencyLevel = stat.ProficiencyLevel,
            TotalHoursSpent = stat.TotalHoursSpent,
            WeakNodeIds = stat.WeakNodeIds,
            UpdatedAt = stat.UpdatedAt
        };
    }
}
