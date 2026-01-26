using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace SSS.Infrastructure.Caching.Redis
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddRedis(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<RedisOptions>(
            configuration.GetSection("Redis"));

            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var redisOptions =
                sp.GetRequiredService<IOptions<RedisOptions>>().Value;

                var config = new ConfigurationOptions
                {
                    EndPoints = { redisOptions.ConnectionString },
                    Password = redisOptions.Password,
                    Ssl = redisOptions.Ssl,
                    AbortOnConnectFail = false
                };

                return ConnectionMultiplexer.Connect(config);
            });

            return services;
        }
    }
}
