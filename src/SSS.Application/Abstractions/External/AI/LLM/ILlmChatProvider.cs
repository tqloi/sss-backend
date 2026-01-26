namespace SSS.Application.Abstractions.External.AI.LLM
{
    public interface ILlmChatProvider
    {
        LlmProvider Provider { get; }
        Task<string> AskAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default);
    }
}
