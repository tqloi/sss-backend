using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.Surveys.GetSurveyBySurveyCode;

public sealed record GetSurveyBySurveyCodeResult(
    bool Success,
    string Message,
    SurveyDto? Data = null
) : GenericResponseRecord<SurveyDto>(Success, Message, Data);