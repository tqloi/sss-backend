using MediatR;

namespace SSS.Application.Features.Surveys.SurveyAnswers.GetAllAnswersByResponse
{
    public sealed record GetAllAnswersByResponseQuery(
        long ResponseId,
        int PageIndex = 1,
        int PageSize = 10) : IRequest<GetAllAnswersByResponseResult>;
}