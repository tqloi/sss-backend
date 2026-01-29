using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.LLM;

namespace SSS.Infrastructure.External.AI
{
    public class LlmRouter : ILlmRouter
    {
        private readonly IDictionary<LlmProvider, ILlmChatProvider> _providers;
        private readonly ILogger<LlmRouter> _logger;

        public LlmRouter(
            IEnumerable<ILlmChatProvider> providers,
            ILogger<LlmRouter> logger)
        {
            _logger = logger;

            _providers = providers.ToDictionary(p => p.Provider);

            _logger.LogInformation(
                "LlmRouter initialized with providers: {Providers}",
                string.Join(", ", _providers.Keys)
            );
        }


        public ILlmChatProvider Resolve(LlmTask task)
        {
            return task switch
            {
                LlmTask.SurveyAnalysis => _providers[LlmProvider.Gemini],
                LlmTask.LearningProfile => _providers[LlmProvider.Gemini],
                LlmTask.SimpleChat => _providers[LlmProvider.Gemini],


                LlmTask.GenerateRoadmap => _providers[LlmProvider.Gpt],
                LlmTask.GenerateStudyPlan => _providers[LlmProvider.Gpt],


                _ => _providers[LlmProvider.Gemini] // default
            };
        }
    }
}
