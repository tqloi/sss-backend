using SSS.Application.Common.Dtos;
using SSS.Application.Features.Surveys.Common;

namespace SSS.Application.Features.Surveys.SurveyAnswers.GetAllAnswersByResponse
{
    public sealed record GetAllAnswersByResponseResult(
        bool Success,
        string Message,
        PaginatedResponse<SurveyAnswerDto>? Data = null);
}   