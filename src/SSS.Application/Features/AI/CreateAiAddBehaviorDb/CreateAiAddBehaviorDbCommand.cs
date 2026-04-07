using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.CreateAiAddBehaviorDb
{
    public sealed record CreateAiAddBehaviorDbCommand
        : IRequest<CreateAiAddBehaviorDbResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;
        public string StudyplanmoduleId { get; set; }

        public string StudyplanId { get; set; }
    }
}