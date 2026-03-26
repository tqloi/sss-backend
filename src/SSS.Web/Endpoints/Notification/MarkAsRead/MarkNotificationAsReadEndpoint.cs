using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System.Security.Claims;

namespace SSS.WebApi.Endpoints.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadEndpoint(
    IAppDbContext dbContext
) : Endpoint<MarkNotificationAsReadRequest, MarkNotificationAsReadResponse>
{
    public override void Configure()
    {
        Post("/api/notifications/{id}/read");
        Description(d => d.WithTags("Notification"));
        Summary(s =>
        {
            s.Summary = "Mark notification as read";
            s.Description = "Marks a specific notification as read for the authenticated user.";
        });
    }

    public override async Task HandleAsync(MarkNotificationAsReadRequest req, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrWhiteSpace(userId))
        {
            await SendUnauthorizedAsync(ct);
            return;
        }

        var entity = await dbContext.UserNotifications
            .FirstOrDefaultAsync(x => x.Id == req.Id && x.UserId == userId, ct);

        if (entity is null)
        {
            await SendNotFoundAsync(ct);
            return;
        }

        if (!entity.IsRead)
        {
            entity.IsRead = true;
            entity.ReadAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(ct);
        }

        await SendOkAsync(new MarkNotificationAsReadResponse
        {
            Id = entity.Id,
            IsRead = entity.IsRead,
            ReadAt = entity.ReadAt
        }, ct);
    }
}
