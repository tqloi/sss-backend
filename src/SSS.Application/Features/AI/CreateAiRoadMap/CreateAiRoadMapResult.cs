using SSS.Application.Features.Content.Roadmaps.Common;
using System.Text.Json;

namespace SSS.Application.Features.AI.CreateAiRoadMap
{
    public sealed class CreateAiRoadMapResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public JsonElement? Rawroadmap { get; set; }
    }
}