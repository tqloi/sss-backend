namespace SSS.Infrastructure.External.AI
{
    public class AiOptions
    {
        public string DefaultProvider { get; set; } = "Gemini";
        public Dictionary<string, string> TaskMapping { get; set; } = new();

        public GeminiAIOptions Gemini { get; set; } = new();
        public OpenAIOptions OpenAI { get; set; } = new();
        public QdrantOptions Qdrant { get; set; } = new();
        public RagOptions Rag { get; set; } = new();

    }

    public class OpenAIOptions
    {
        public string ApiKey { get; set; } = string.Empty;
        public string ChatModel { get; set; } = "gpt-4o-mini";
        public string EmbeddingModel { get; set; } = "text-embedding-3-small";
    }
    public class GeminiAIOptions
    {
        public string ApiKey { get; set; } = default!;
        public string Model { get; set; } = "gemini-1.5-flash";
        public string BaseUrl { get; set; } = "https://generativelanguage.googleapis.com/v1beta/";
    }

    public class QdrantOptions
    {
        public string Endpoint { get; set; } = "http://localhost:6333";
        public string Collection { get; set; } = "user_surveys";
    }

    public class RagOptions
    {
        public int ChunkSize { get; set; } = 800;
        public int ChunkOverlap { get; set; } = 120;
        public int TopK { get; set; } = 5;
    }
}
