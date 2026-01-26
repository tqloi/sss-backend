using MediatR;

namespace SSS.Application.Features.Surveys.SurveyAnswers.GetAnswerByQuestion
{
    public sealed record GetAnswerByQuestionQuery(
        long ResponseId,
        long QuestionId) : IRequest<GetAnswerByQuestionResult>;
}