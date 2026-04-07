using SSS.Domain.Entities.Identity;

namespace SSS.Application.Abstractions.Services
{
    public interface ISubscriptionService
    {
        bool IsPremium(User user);
        int GetCourseLimit(User user);
        string GetAiModel(User user);
    }
}
