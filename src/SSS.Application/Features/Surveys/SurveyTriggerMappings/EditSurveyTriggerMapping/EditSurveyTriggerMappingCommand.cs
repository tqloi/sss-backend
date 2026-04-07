using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.EditSurveyTriggerMapping
{
    public sealed record EditSurveyTriggerMappingCommand: IRequest<EditSurveyTriggerMappingResponse>
    {
        public long Id { get; set; }

        public long SurveyId { get; set; }

        public string TriggerType { get; set; } = default!;

        public int? MaxAttempts { get; set; }

        public int? CooldownDays { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
