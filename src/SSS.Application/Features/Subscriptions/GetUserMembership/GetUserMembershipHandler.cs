using MediatR;
using Microsoft.AspNetCore.Identity;
using SSS.Application.Features.Subscriptions.Common;
using SSS.Domain.Entities.Identity;
using SSS.Domain.Enums;

namespace SSS.Application.Features.Subscriptions.GetUserMembership;

public sealed class GetUserMembershipHandler(
    UserManager<User> userManager
) : IRequestHandler<GetUserMembershipQuery, UserMembershipDto>
{
    public async Task<UserMembershipDto> Handle(GetUserMembershipQuery request, CancellationToken ct)
    {
        var user = await userManager.FindByIdAsync(request.UserId);
        if (user is null)
        {
            throw new InvalidOperationException("User not found");
        }

        var todayUtc = DateTime.UtcNow.Date;
        var subscriptionEndDateUtc = user.SubscriptionEndDate?.Date;

        var hasPaidPlan =
            user.SubscriptionType.HasValue &&
            user.SubscriptionType.Value != SubscriptionType.Free;

        // Treat membership as active for the whole expiration day.
        var hasActiveSubscription =
            hasPaidPlan &&
            subscriptionEndDateUtc.HasValue &&
            subscriptionEndDateUtc.Value >= todayUtc;

        var daysRemaining =
            hasActiveSubscription && subscriptionEndDateUtc.HasValue
                ? (subscriptionEndDateUtc.Value - todayUtc).Days + 1
                : 0;

        return new UserMembershipDto
        {
            UserId = user.Id,
            Email = user.Email ?? string.Empty,
            SubscriptionType = user.SubscriptionType?.ToString(),
            SubscriptionStartDate = user.SubscriptionStartDate,
            SubscriptionEndDate = user.SubscriptionEndDate,
            HasActiveSubscription = hasActiveSubscription,
            DaysRemaining = daysRemaining,
        };
    }
}
