using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyAnswers.UpdateAnswer
{
    public sealed record UpdateAnswerResponse(
        bool Success,
        string Message,
        SurveyAnswerDto? Data = null) : GenericResponseRecord<SurveyAnswerDto>(Success, Message, Data);
}