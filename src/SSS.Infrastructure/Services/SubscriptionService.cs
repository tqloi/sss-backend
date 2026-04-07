using SSS.Application.Abstractions.Services;
using SSS.Domain.Entities.Identity;

namespace SSS.Infrastructure.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        public bool IsPremium(User user)
            => user.SubscriptionEndDate > DateTime.UtcNow;

        public int GetCourseLimit(User user)
            => IsPremium(user) ? 20 : 1;

        public string GetAiModel(User user)
            => IsPremium(user) ? "gpt-4" : "gpt-3.5";
    }
}
