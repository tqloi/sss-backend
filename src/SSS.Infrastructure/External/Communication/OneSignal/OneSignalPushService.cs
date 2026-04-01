using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSS.Application.Abstractions.External.Communication.Push;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace SSS.Infrastructure.External.Communication.OneSignal;

public sealed class OneSignalPushService(
    IHttpClientFactory httpClientFactory,
    IOptions<OneSignalOptions> options,
    ILogger<OneSignalPushService> logger
) : IOneSignalPushService
{
    private readonly OneSignalOptions _options = options.Value;

    public async Task<int> SendToSubscriptionIdsAsync(
        IReadOnlyCollection<string> subscriptionIds,
        string title,
        string content,
        IReadOnlyDictionary<string, string>? data = null,
        CancellationToken ct = default)
    {
        if (subscriptionIds.Count == 0)
            return 0;

        if (string.IsNullOrWhiteSpace(_options.AppId)
            || string.IsNullOrWhiteSpace(_options.ApiKey))
        {
            logger.LogWarning("[OneSignal] AppId/ApiKey is missing. Push request skipped.");
            return 0;
        }

        var client = httpClientFactory.CreateClient();
        client.BaseAddress = new Uri(_options.BaseUrl.TrimEnd('/') + "/");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Key", _options.ApiKey);

        var request = new OneSignalCreateNotificationRequest
        {
            AppId = _options.AppId,
            IncludeSubscriptionIds = subscriptionIds,
            Headings = new Dictionary<string, string> { ["en"] = title },
            Contents = new Dictionary<string, string> { ["en"] = content },
            Data = data is null ? null : new Dictionary<string, string>(data)
        };

        using var response = await client.PostAsJsonAsync("notifications", request, ct);
        if (!response.IsSuccessStatusCode)
        {
            var body = await response.Content.ReadAsStringAsync(ct);
            logger.LogWarning(
                "[OneSignal] Push failed. Status={StatusCode}, Body={Body}",
                (int)response.StatusCode,
                body);
            return 0;
        }

        logger.LogInformation("[OneSignal] Push sent to {Count} subscription ids.", subscriptionIds.Count);
        return subscriptionIds.Count;
    }

    private sealed class OneSignalCreateNotificationRequest
    {
        [JsonPropertyName("app_id")]
        public string AppId { get; set; } = string.Empty;

        [JsonPropertyName("include_subscription_ids")]
        public IReadOnlyCollection<string> IncludeSubscriptionIds { get; set; } = Array.Empty<string>();

        public Dictionary<string, string> Headings { get; set; } = new();
        public Dictionary<string, string> Contents { get; set; } = new();
        public Dictionary<string, string>? Data { get; set; }
    }
}
