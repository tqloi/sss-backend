namespace SSS.Infrastructure.External.Communication.OneSignal;

public sealed class OneSignalOptions
{
    public string BaseUrl { get; set; } = "https://api.onesignal.com";
    public string AppId { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
}
