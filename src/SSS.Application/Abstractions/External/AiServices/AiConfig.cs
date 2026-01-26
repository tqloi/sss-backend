using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Abstractions.External.AiServices
{
    public class AiConfig
    {
        public string Provider { get; set; } = "OpenAI";
        public GeminiAIConfig Gemini { get; set; } = new();
        public OpenAIConfig OpenAI { get; set; } = new();
        public QdrantConfig Qdrant { get; set; } = new();
        public RagConfig Rag { get; set; } = new();

    }

    public class OpenAIConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatModel { get; set; } = "gpt-4o-mini";
        public string EmbeddingModel { get; set; } = "text-embedding-3-small";
    }
    public class GeminiAIConfig
    {
        public string ApiKey { get; set; } = string.Empty;
        public string Model { get; set; } = "gemini-1.5-flash";
    }

    public class QdrantConfig
    {
        public string Endpoint { get; set; } = "http://localhost:6333";
        public string Collection { get; set; } = "user_surveys";
    }

    public class RagConfig
    {
        public int ChunkSize { get; set; } = 800;
        public int ChunkOverlap { get; set; } = 120;
        public int TopK { get; set; } = 5;
    }
}
