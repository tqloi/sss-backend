using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Abstractions.External.AI.LLM
{
    public interface ILlmChatProvider
    {
        Task<string> AskAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken = default);
    }
}
