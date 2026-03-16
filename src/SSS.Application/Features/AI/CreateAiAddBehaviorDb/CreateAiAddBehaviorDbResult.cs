namespace SSS.Application.Features.AI.CreateAiAddBehaviorDb
{
    public sealed record CreateAiAddBehaviorDbResult
    {
        public bool Success { get; init; }
        public string? Message { get; init; }
    }
}