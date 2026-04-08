namespace SSS.Application.Features.AI.GetModuleBehaviorInsight
{
    public sealed record GetModuleBehaviorInsightResult
    {
        public bool Success { get; init; }

        public string Message { get; init; } = string.Empty;

        public string? Insight { get; init; }
    }
}
