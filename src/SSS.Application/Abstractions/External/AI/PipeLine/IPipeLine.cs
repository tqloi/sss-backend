using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Abstractions.External.AI.PipeLine
{
    public interface IPipeLine
    {
        Task IngestAsync(string userId, IEnumerable<(string Text, string? Source)> chunks, CancellationToken ct = default);
        Task<string> AskAsync(string question, int? topK, CancellationToken ct = default);

        Task<string> BuildStudyPlanContextAsync(string userId, CancellationToken ct = default);
        Task<string> GenerateStudyPlanAsync(string userId, object roadmap, CancellationToken ct = default);
    }
}
