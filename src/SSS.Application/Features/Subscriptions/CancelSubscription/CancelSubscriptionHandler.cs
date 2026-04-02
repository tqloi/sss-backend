using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Common.Exceptions;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Subscriptions.CancelSubscription;

public sealed class CancelSubscriptionHandler(
    IAppDbContext context
) : IRequestHandler<CancelSubscriptionCommand, bool>
{
    public async Task<bool> Handle(CancelSubscriptionCommand request, CancellationToken ct)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user == null)
        {
            throw new NotFoundException("User not found.");
        }

        if (user.SubscriptionType == SubscriptionType.Free)
        {
            return false; // Already free
        }

        // Revoke premium privileges immediately according to system design
        user.SubscriptionType = SubscriptionType.Free;
        user.SubscriptionStartDate = null;
        user.SubscriptionEndDate = null; 

        await context.SaveChangesAsync(ct);
        return true;
    }
}
