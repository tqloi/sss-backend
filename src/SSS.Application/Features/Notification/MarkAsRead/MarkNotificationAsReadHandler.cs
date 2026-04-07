using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Notification.MarkAsRead;

public sealed class MarkNotificationAsReadHandler(
    IAppDbContext dbContext
) : IRequestHandler<MarkNotificationAsReadCommand, MarkNotificationAsReadResult>
{
    public async Task<MarkNotificationAsReadResult> Handle(MarkNotificationAsReadCommand request, CancellationToken ct)
    {
        var entity = await dbContext.UserNotifications
            .FirstOrDefaultAsync(x => x.Id == request.NotificationId && x.UserId == request.UserId, ct);

        if (entity is null)
        {
            return new MarkNotificationAsReadResult
            {
                Found = false,
                Id = request.NotificationId,
                IsRead = false,
                ReadAt = null
            };
        }

        if (!entity.IsRead)
        {
            entity.IsRead = true;
            entity.ReadAt = DateTime.UtcNow;
            await dbContext.SaveChangesAsync(ct);
        }

        return new MarkNotificationAsReadResult
        {
            Found = true,
            Id = entity.Id,
            IsRead = entity.IsRead,
            ReadAt = entity.ReadAt
        };
    }
}
