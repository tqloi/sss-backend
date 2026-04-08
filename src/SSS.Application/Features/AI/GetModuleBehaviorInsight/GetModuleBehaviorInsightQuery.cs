using MediatR;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.AI.GetModuleBehaviorInsight
{
    public sealed record GetModuleBehaviorInsightQuery : IRequest<GetModuleBehaviorInsightResult>
    {
        [JsonIgnore]
        public string UserId { get; set; } = null!;

        public long StudyPlanId { get; set; }
    }
}
