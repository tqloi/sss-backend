using Microsoft.Extensions.Options;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.LLM;

namespace SSS.Infrastructure.External.AI
{
    public class LlmRouter : ILlmRouter
    {
        private readonly IDictionary<LlmProvider, ILlmChatProvider> _providers;


        public LlmRouter(IEnumerable<ILlmChatProvider> providers)
        {
            _providers = providers.ToDictionary(p => p.Provider);
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
