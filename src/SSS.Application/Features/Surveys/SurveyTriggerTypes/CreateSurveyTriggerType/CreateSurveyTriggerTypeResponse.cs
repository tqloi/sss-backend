using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.CreateSurveyTriggerType
{
    public sealed record CreateSurveyTriggerTypeResponse(
        bool Success,
        string Message,
        SurveyTriggerTypeDto? Data = null
    ) : GenericResponseRecord<SurveyTriggerTypeDto>(Success, Message, Data);
}
