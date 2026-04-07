using SSS.Application.Abstractions.External.AI.LLM;

namespace SSS.Application.Abstractions.External.AI
{
    public interface ILlmRouter
    {
        ILlmChatProvider Resolve(LlmTask task);
    }
}
