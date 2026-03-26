using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;

namespace SSS.Application.Features.Notification.DeactivatePushToken;

public sealed class DeactivatePushTokenHandler(
    IAppDbContext dbContext
) : IRequestHandler<DeactivatePushTokenCommand, DeactivatePushTokenResult>
{
    public async Task<DeactivatePushTokenResult> Handle(DeactivatePushTokenCommand request, CancellationToken ct)
    {
        var token = request.DeviceToken.Trim();
        var now = DateTime.UtcNow;

        var entity = await dbContext.UserPushTokens
            .FirstOrDefaultAsync(x => x.UserId == request.UserId && x.DeviceToken == token, ct);

        if (entity is null)
        {
            return new DeactivatePushTokenResult
            {
                Deactivated = false,
                UserId = request.UserId,
                DeviceToken = token,
                LastUpdated = now
            };
        }

        entity.IsActive = false;
        entity.LastUpdated = now;

        await dbContext.SaveChangesAsync(ct);

        return new DeactivatePushTokenResult
        {
            Deactivated = true,
            UserId = entity.UserId,
            DeviceToken = entity.DeviceToken,
            LastUpdated = entity.LastUpdated
        };
    }
}
