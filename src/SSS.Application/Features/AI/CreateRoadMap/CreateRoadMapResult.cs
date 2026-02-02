using SSS.Application.Features.Content.Roadmaps.Common;
using System.Text.Json;

namespace SSS.Application.Features.AI.CreateRoadMap
{
    public sealed class CreateRoadMapResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
        public JsonElement? Rawroadmap { get; set; }
    }


}