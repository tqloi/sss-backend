using SSS.Domain.Entities.Assessment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyTriggerMappingDto
    (
        long Id,
        long SurveyId,
        string TriggerType,
        int? MaxAttempts,
        int? CooldownDays,
        bool IsActive,
        DateTime CreatedAt
    );
    
}
