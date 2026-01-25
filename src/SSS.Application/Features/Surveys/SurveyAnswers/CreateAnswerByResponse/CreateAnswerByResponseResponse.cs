using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyAnswers.CreateAnswerByResponse
{
    public sealed record CreateAnswerByResponseResponse(
        bool Success,
        string Message,
        SurveyAnswerDto? Data = null) : GenericResponseRecord<SurveyAnswerDto>(Success, Message, Data);
}