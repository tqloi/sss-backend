using OpenAI.Chat;
using SSS.Application.Abstractions.External.AI.LLM;

namespace SSS.Infrastructure.External.AI.OpenAI.LLM
{
    public class OpenAIChatProvider : ILlmChatProvider
    {
        private readonly ChatClient _chatClient;

        public OpenAIChatProvider(ChatClient chatClient)
        {
            _chatClient = chatClient;
        }
        public async Task<string> AskAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default)
        {
            var messages = new List<ChatMessage>
            {
                new SystemChatMessage(systemPrompt),
                new UserChatMessage(userPrompt)
            };

            var completions = await _chatClient.CompleteChatAsync(
                messages: messages,
                cancellationToken: cancellationToken
            );
            return completions.Value.Content.Count > 0 ? completions.Value.Content[0].Text.ToString() : string.Empty;
        }
    }
}
