using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.UserSubjectStats.GetMy;

public sealed class GetMyUserSubjectStatsEndpoint(
    IAppDbContext dbContext
) : EndpointWithoutRequest<GetMyUserSubjectStatsResponse>
{
    public override void Configure()
    {
        Get("/api/user-subject-stats/me");
        Description(d => d.WithTags("UserSubjectStats"));
        Summary(s =>
        {
            s.Summary = "Get my subject stats";
            s.Description = "Returns all subject statistics for the authenticated user.";
        });
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var stats = await dbContext.UserSubjectStats
            .Include(s => s.Subject)
            .Where(s => s.UserId == userId)
            .OrderByDescending(s => s.UpdatedAt)
            .Select(s => new GetMyUserSubjectStatsResponse.UserSubjectStatItem
            {
                Id = s.Id,
                SubjectId = s.SubjectId,
                SubjectName = s.Subject.Name,
                ProficiencyLevel = s.ProficiencyLevel,
                TotalHoursSpent = s.TotalHoursSpent,
                WeakNodeIds = s.WeakNodeIds,
                UpdatedAt = s.UpdatedAt
            })
            .ToListAsync(ct);

        var response = new GetMyUserSubjectStatsResponse { Items = stats };
        await SendOkAsync(response, ct);
    }
}
