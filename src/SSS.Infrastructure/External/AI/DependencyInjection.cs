using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Namotion.Reflection;
using OpenAI.Chat;
using OpenAI.Embeddings;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.Embedding;
using SSS.Application.Abstractions.External.AI.LLM;
using SSS.Application.Abstractions.External.AI.PipeLine;
using SSS.Application.Abstractions.External.AI.Vector;
using SSS.Infrastructure.External.AI.Embedding;
using SSS.Infrastructure.External.AI.LLM;
using SSS.Infrastructure.External.AI.PipeLine;
using SSS.Infrastructure.External.AI.Vector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Infrastructure.External.AI
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAIService(
            this IServiceCollection services,
            IConfiguration config
        )
        {
            var configuration = config.Get<AiConfig>() ?? new AiConfig();
            services.AddSingleton(configuration);
            services.AddHttpClient();
            if (configuration.Provider == "OpenAI")
            {
                services.AddScoped<ILlmChatProvider, OpenAIChatProvider>();
                services.AddSingleton(
                    new ChatClient(configuration.OpenAI.ChatModel, configuration.OpenAI.ApiKey));
                services.AddSingleton<IEmbeddingProvider, OpenAIEmbeddingProvider>();
                services.AddSingleton(
                    new EmbeddingClient(configuration.OpenAI.EmbeddingModel, configuration.OpenAI.ApiKey));
            }
            else if (configuration.Provider == "Gemini")
            {
                services.AddHttpClient();

                services.AddScoped<ILlmChatProvider>(sp =>
                {
                    var http = sp.GetRequiredService<HttpClient>();
                    return new GeminiChatProvider(
                        http,
                        configuration.Gemini.ApiKey,
                        configuration.Gemini.Model
                    );
                });
            }
            services.AddSingleton<IQdrantClient, QdrantClient>();
            services.AddScoped<IPipeLine,RagPipeline>();

            return services;
        }
    }
}
