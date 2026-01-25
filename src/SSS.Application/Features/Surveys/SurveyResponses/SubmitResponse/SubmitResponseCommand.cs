using MediatR;
using SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
    }
}
