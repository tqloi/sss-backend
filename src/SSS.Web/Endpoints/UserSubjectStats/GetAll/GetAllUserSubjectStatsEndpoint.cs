using FastEndpoints;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.WebApi.Endpoints.UserSubjectStats.GetAll;

public sealed class GetAllUserSubjectStatsEndpoint(
    IAppDbContext dbContext,
    AutoMapper.IMapper mapper
) : EndpointWithoutRequest<GetAllUserSubjectStatsResponse>
{
    public override void Configure()
    {
        Get("/api/user-subject-stats");
        Roles("Admin");
        Tags("UserSubjectStats");
        Summary(s =>
        {
            s.Summary = "Get all user subject stats (Admin only)";
            s.Description = "Returns all subject statistics. Requires Admin role.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var stats = await dbContext.UserSubjectStats
            .Include(s => s.Subject)
            .OrderByDescending(s => s.UpdatedAt)
            .Select(s => new GetAllUserSubjectStatsResponse.UserSubjectStatItem
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

        var response = new GetAllUserSubjectStatsResponse { Items = stats };
        await SendOkAsync(response, ct);
    }
}
