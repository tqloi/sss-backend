using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SSS.Application.Features.AI.CreateAiTaskItems
{
    public sealed record CreateAiTaskItemsResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public JsonElement? RawTaskItens { get; set; }
    }
}
