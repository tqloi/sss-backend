using MediatR;

namespace SSS.Application.Features.Surveys.SurveyAnswers.DeleteAnswer
{
    public sealed record DeleteAnswerCommand(long AnswerId) : IRequest<DeleteAnswerResponse>;
}