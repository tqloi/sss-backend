using Microsoft.Extensions.Logging;
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
        private readonly ILogger<GeminiChatProvider> _logger;
        public LlmProvider Provider => LlmProvider.Gemini;

        public GeminiChatProvider(
            HttpClient httpClient,
            IOptions<GeminiAIOptions> options,
            ILogger<GeminiChatProvider> logger)
        {
            _httpClient = httpClient;
            _options = options.Value;
            _logger = logger;
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

            // TrimEnd('/') prevents double-slash when BaseUrl already ends with '/'
            var baseUrl = _options.BaseUrl.TrimEnd('/');
            var url = $"{baseUrl}/models/{_options.Model}:generateContent?key={_options.ApiKey}";

            var json = JsonSerializer.Serialize(requestBody);
            using var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };

            _logger.LogDebug("Calling Gemini model {Model}", _options.Model);

            var response = await _httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError(
                    "Gemini request failed. Status: {StatusCode} ({StatusCodeInt}). Body: {Body}",
                    response.StatusCode,
                    (int)response.StatusCode,
                    responseBody);

                throw new InvalidOperationException(
                    $"Gemini error [{(int)response.StatusCode} {response.StatusCode}]: {responseBody}");
            }

            using var document = JsonDocument.Parse(responseBody);
            var root = document.RootElement;

            // Handle blocked/empty candidates
            if (!root.TryGetProperty("candidates", out var candidates) || candidates.GetArrayLength() == 0)
            {
                _logger.LogWarning("Gemini returned no candidates. Body: {Body}", responseBody);
                throw new InvalidOperationException("Gemini returned no candidates.");
            }

            var text = candidates[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return text ?? "No response from Gemini.";
        }
    }
}