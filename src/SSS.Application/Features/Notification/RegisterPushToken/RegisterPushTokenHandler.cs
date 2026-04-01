using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Domain.Entities.Notification;

namespace SSS.Application.Features.Notification.RegisterPushToken;

public sealed class RegisterPushTokenHandler(
    IAppDbContext dbContext
) : IRequestHandler<RegisterPushTokenCommand, RegisterPushTokenResult>
{
    public async Task<RegisterPushTokenResult> Handle(RegisterPushTokenCommand request, CancellationToken ct)
    {
        var token = request.DeviceToken.Trim();
        var deviceType = request.DeviceType.Trim().ToLowerInvariant();
        var now = DateTime.UtcNow;

        var existing = await dbContext.UserPushTokens
            .FirstOrDefaultAsync(x => x.DeviceToken == token, ct);

        if (existing is null)
        {
            existing = new UserPushToken
            {
                UserId = request.UserId,
                DeviceToken = token,
                DeviceType = deviceType,
                IsActive = true,
                LastUpdated = now
            };

            dbContext.UserPushTokens.Add(existing);
        }
        else
        {
            existing.UserId = request.UserId;
            existing.DeviceType = deviceType;
            existing.IsActive = true;
            existing.LastUpdated = now;
        }

        await dbContext.SaveChangesAsync(ct);

        return new RegisterPushTokenResult
        {
            Id = existing.Id,
            UserId = existing.UserId,
            DeviceToken = existing.DeviceToken,
            DeviceType = existing.DeviceType,
            IsActive = existing.IsActive,
            LastUpdated = existing.LastUpdated
        };
    }
}
