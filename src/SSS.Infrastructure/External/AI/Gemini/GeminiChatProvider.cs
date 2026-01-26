using Microsoft.Extensions.Options;
using SSS.Application.Abstractions.External.AI;
using SSS.Application.Abstractions.External.AI.LLM;
using System.Text;
using System.Text.Json;

namespace SSS.Infrastructure.External.AI.Gemini
{
    public class GeminiChatProvider : ILlmChatProvider
    {
        private readonly HttpClient _httpClient;
        private readonly GeminiAIOptions _options;
        public LlmProvider Provider => LlmProvider.Gemini;

        public GeminiChatProvider(
        HttpClient httpClient,
        IOptions<GeminiAIOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<string> AskAsync(
        string systemPrompt,
        string userPrompt,
        CancellationToken cancellationToken = default)
        {
            var requestBody = new
            {
                systemInstruction = new
                {
                    parts = new[] { new { text = systemPrompt } }
                },
                contents = new[]
                {
                    new
                    {
                        role = "user",
                        parts = new[] { new { text = userPrompt } }
                    }
                }
            };

            var request = new HttpRequestMessage(
            HttpMethod.Post,
            $"models/{_options.Model}:generateContent");

            request.Headers.Add("X-Goog-Api-Key", _options.ApiKey);
            request.Content = new StringContent(
            JsonSerializer.Serialize(requestBody),
            Encoding.UTF8,
            "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync(cancellationToken);
                throw new InvalidOperationException($"Gemini error: {err}");
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(
            stream, cancellationToken: cancellationToken);

            return document.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString()
            ?? "No response from Gemini.";
        }
    }
}