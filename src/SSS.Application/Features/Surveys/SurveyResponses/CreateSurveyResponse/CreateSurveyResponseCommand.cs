using MediatR;
using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyResponses.CreateSurveyResponse
{
    public sealed record CreateSurveyResponseCommand : IRequest<CreateSurveyResponseResponse>
    {
        public long SurveyId { get; init; }

        [JsonIgnore]
        public string UserId { get; init; } = default!;

        public DateTime StartedAt { get; init; } = DateTime.Now;
        
        //public string? SnapshotJson { get; init; }
        public SurveyTriggerReason TriggerReason { get; init; }
    }
    
    
}
