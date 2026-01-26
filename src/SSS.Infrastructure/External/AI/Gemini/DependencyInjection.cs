using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SSS.Application.Abstractions.External.AI.LLM;

namespace SSS.Infrastructure.External.AI.Gemini
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddGemini(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            // Bind Gemini options
            services.Configure<GeminiOptions>(
            configuration.GetSection("Gemini"));

            // Typed HttpClient
            services.AddHttpClient<GeminiChatProvider>((sp, client) =>
            {
                var options = sp
                .GetRequiredService<IOptions<GeminiOptions>>()
                .Value;

                client.BaseAddress = new Uri(options.BaseUrl);
                client.DefaultRequestHeaders.Add(
                "Accept", "application/json");
            });

            // Bind abstraction
            //services.AddScoped<ILlmChatProvider, GeminiChatProvider>();

            return services;
        }
    }
}
