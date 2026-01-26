using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Abstractions.External.AI.Embedding
{
    public interface IEmbeddingProvider
    {
        Task<float[]> EmbeddingAsync(string input, CancellationToken ct = default);
        Task<int> GetDimAsync(CancellationToken ct = default);
    }
}
