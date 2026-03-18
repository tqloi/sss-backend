using MediatR;
using SSS.Domain.Enums;
using System.Text.Json.Serialization;

namespace SSS.Application.Features.Surveys.SurveyResponses.SubmitResponse
{
    public sealed record SubmitResponseCommand : IRequest<SubmitResponseResponse>
    {
        public long ResponseId { get; init; }
        public long SurveyId { get; init; }

        [JsonIgnore]
        public string UserId { get; init; } = default!;

        public DateTime SubmittedAt { get; init; } = DateTime.UtcNow;

        public SurveyTriggerReason TriggerReason { get; init; }

        /// <summary>
        /// Required only when submitting a ROADMAP_LEARNING_TARGET survey.
        /// The roadmap the user is targeting — used to create the StudyPlan.
        /// </summary>
        public long? RoadmapId { get; init; }
    }
}
