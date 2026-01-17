using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SSS.Infrastructure.External.Payment.PayOS
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPayOSService(
         this IServiceCollection services,
         IConfiguration config)
        {
            var section = config.GetSection("PayOS");

            // Lấy thông tin từ appsettings.json
            var clientId = section["ClientId"]
                ?? throw new InvalidOperationException("PayOS:ClientId is missing in configuration.");
            var apiKey = section["ApiKey"]
                ?? throw new InvalidOperationException("PayOS:ApiKey is missing in configuration.");
            var checksumKey = section["ChecksumKey"]
                ?? throw new InvalidOperationException("PayOS:ChecksumKey is missing in configuration.");

            // Đăng ký SDK PayOS trực tiếp (Singleton)
            services.AddSingleton(_ => new Net.payOS.PayOS(clientId, apiKey, checksumKey));

            return services;
        }
    }
}
