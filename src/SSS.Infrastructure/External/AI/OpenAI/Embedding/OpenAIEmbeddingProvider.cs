using OpenAI.Embeddings;
using SSS.Application.Abstractions.External.AI.Embedding;

namespace SSS.Infrastructure.External.AI.OpenAI.Embedding
{
    public class OpenAIEmbeddingProvider : IEmbeddingProvider
    {
        private readonly EmbeddingClient _client;
        private int? _dim;
        public OpenAIEmbeddingProvider(EmbeddingClient client)
        {
            _client = client;
        }
        public async Task<float[]> EmbeddingAsync(string input, CancellationToken ct = default)
        {
            var result = await _client.GenerateEmbeddingAsync(
                input,
                cancellationToken: ct
            );

            var vec = result.Value.ToFloats().ToArray();
            _dim ??= vec.Length;
            return vec;
        }

        public async Task<int> GetDimAsync(CancellationToken ct = default)
        {
            if (_dim.HasValue) return _dim.Value;
            _ = await EmbeddingAsync("dimension probe", ct);
            return _dim!.Value;
        }
    }
}
