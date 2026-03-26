using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SSS.Infrastructure.External.Communication.OneSignal;

public static class DependencyInjection
{
    public static IServiceCollection AddOneSignal(
        this IServiceCollection services,
        IConfiguration config)
    {
        services.Configure<OneSignalOptions>(config.GetSection("OneSignal"));
        services.AddHttpClient();
        return services;
    }
}
