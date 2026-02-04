using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.Surveys.SurveyTriggerMappings.CreateSurveyTriggerMapping
{
    public sealed record CreateSurveyTriggerMappingResponse(
            bool Success,
            string Message,
            SurveyTriggerMappingDto? Data = null) : GenericResponseRecord<SurveyTriggerMappingDto>(Success, Message, Data);
}
