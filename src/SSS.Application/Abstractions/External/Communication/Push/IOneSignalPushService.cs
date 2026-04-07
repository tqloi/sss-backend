namespace SSS.Application.Abstractions.External.Communication.Push;

public interface IOneSignalPushService
{
    Task<int> SendToSubscriptionIdsAsync(
        IReadOnlyCollection<string> subscriptionIds,
        string title,
        string content,
        IReadOnlyDictionary<string, string>? data = null,
        CancellationToken ct = default);
}
