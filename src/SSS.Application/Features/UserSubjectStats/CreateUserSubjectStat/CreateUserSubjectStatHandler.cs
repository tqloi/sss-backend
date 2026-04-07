using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.UserSubjectStats.Common;
using SSS.Domain.Entities.Tracking;

namespace SSS.Application.Features.UserSubjectStats.CreateUserSubjectStat;

public sealed class CreateUserSubjectStatHandler(
    IAppDbContext dbContext
) : IRequestHandler<CreateUserSubjectStatCommand, UserSubjectStatDto>
{
    public async Task<UserSubjectStatDto> Handle(CreateUserSubjectStatCommand request, CancellationToken ct)
    {
        // Check if already exists (unique constraint: UserId + SubjectId)
        var exists = await dbContext.UserSubjectStats
            .AnyAsync(s => s.UserId == request.UserId && s.SubjectId == request.SubjectId, ct);

        if (exists)
            throw new InvalidOperationException("User already has stats for this subject.");

        // Verify subject exists
        var subject = await dbContext.LearningSubjects
            .FirstOrDefaultAsync(s => s.Id == request.SubjectId, ct);

        if (subject is null)
            throw new InvalidOperationException("Subject not found.");

        var stat = new UserSubjectStat
        {
            UserId = request.UserId,
            SubjectId = request.SubjectId,
            ProficiencyLevel = request.ProficiencyLevel,
            TotalHoursSpent = request.TotalHoursSpent,
            WeakNodeIds = string.IsNullOrWhiteSpace(request.WeakNodeIds) ? null : request.WeakNodeIds,
            UpdatedAt = DateTime.UtcNow
        };

        dbContext.UserSubjectStats.Add(stat);
        await dbContext.SaveChangesAsync(ct);

        return new UserSubjectStatDto
        {
            Id = stat.Id,
            UserId = stat.UserId,
            SubjectId = stat.SubjectId,
            SubjectName = subject.Name,
            ProficiencyLevel = stat.ProficiencyLevel,
            TotalHoursSpent = stat.TotalHoursSpent,
            WeakNodeIds = stat.WeakNodeIds,
            UpdatedAt = stat.UpdatedAt
        };
    }
}
