using SSS.Application.Abstractions.External.AiServices.LLM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSS.Infrastructure.External.AiServices.LLM
{
    public class GeminiChatProvider : ILlmChatProvider
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _model;

        public GeminiChatProvider(HttpClient httpClient, string apiKey, string model)
        {
            _httpClient = httpClient;
            _apiKey = apiKey;
            _model = model;
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
                    parts = new[]
        {
            new { text = systemPrompt }
        }
                },
                contents = new[]
        {
        new
        {
                role = "user",
                parts = new[]
                {
                    new { text = userPrompt }
                }
            }
        }
            };

            var request = new HttpRequestMessage(
                HttpMethod.Post,
                $"https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent");

            request.Headers.Add("X-Goog-Api-Key", _apiKey);
            request.Content = new StringContent(
                JsonSerializer.Serialize(requestBody),
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                var err = await response.Content.ReadAsStringAsync();
                return $"Gemini error: {err}";
            }

            using var stream = await response.Content.ReadAsStreamAsync(cancellationToken);
            using var document = await JsonDocument.ParseAsync(stream, cancellationToken: cancellationToken);

            var text = document.RootElement
                .GetProperty("candidates")[0]
                .GetProperty("content")
                .GetProperty("parts")[0]
                .GetProperty("text")
                .GetString();

            return string.IsNullOrWhiteSpace(text) ? "No response from Gemini." : text;
        }
    }
}