using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.Embedding;
using SSS.Application.Abstractions.External.AI.LLM;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.External.AI.Vector;
using SSS.Infrastructure.External.AI.OpenAI.Embedding;
using SSS.Infrastructure.External.AI.OpenAI.LLM;
using SSS.Infrastructure.External.AI.OpenAI.PipeLine;
using SSS.Infrastructure.External.AI.OpenAI.Vector;

namespace SSS.Infrastructure.External.AI.OpenAI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddOpenAI(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            var configuration = config.Get<AiConfig>() ?? new AiConfig();
            services.AddSingleton(configuration);
            services.AddHttpClient();

            services.AddScoped<ILlmChatProvider, OpenAIChatProvider>();
            services.AddSingleton(
                new ChatClient(configuration.OpenAI.ChatModel, configuration.OpenAI.ApiKey));
            services.AddSingleton<IEmbeddingProvider, OpenAIEmbeddingProvider>();
            services.AddSingleton(
                new EmbeddingClient(configuration.OpenAI.EmbeddingModel, configuration.OpenAI.ApiKey));

            services.AddHttpClient();

            services.AddSingleton<IQdrantClient, QdrantClient>();
            services.AddScoped<IPipeLine, RagPipeline>();

            return services;
        }
    }
}
