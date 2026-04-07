using SSS.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.Common
{
    public sealed record SurveyResponseDto
    (
        
        long SurveyId,
        string UserId,
        DateTime StartedAt,
        DateTime? SubmittedAt,
        string? SnapshotJson,
        SurveyTriggerReason TriggerReason

    );
}
