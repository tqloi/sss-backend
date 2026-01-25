using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyAnswers.DeleteAnswer
{
    public sealed record DeleteAnswerResponse(
        bool Success,
        string Message) : GenericResponseRecord(Success, Message);
}