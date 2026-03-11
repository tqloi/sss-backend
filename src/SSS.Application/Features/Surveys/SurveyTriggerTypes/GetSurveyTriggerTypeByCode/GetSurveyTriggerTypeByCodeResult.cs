using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyTriggerTypes.GetSurveyTriggerTypeByCode
{
    public sealed record GetSurveyTriggerTypeByCodeResult(
        bool Success,
        string Message,
        SurveyTriggerTypeDto? Data = null
    ) : GenericResponseRecord<SurveyTriggerTypeDto>(Success, Message, Data);
}
