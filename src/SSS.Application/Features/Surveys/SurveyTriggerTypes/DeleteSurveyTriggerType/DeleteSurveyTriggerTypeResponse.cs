using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.DeleteSurveyTriggerType
{
    public sealed record DeleteSurveyTriggerTypeResponse(
        bool Success,
        string Message,
        SurveyTriggerTypeDto? Data = null
    ) : GenericResponseRecord<SurveyTriggerTypeDto>(Success, Message, Data);
}
