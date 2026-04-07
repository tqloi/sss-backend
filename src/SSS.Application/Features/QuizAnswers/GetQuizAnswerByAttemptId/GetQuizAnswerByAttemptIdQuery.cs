using MediatR;

namespace SSS.Application.Features.QuizAnswers.GetQuizAnswerByAttemptId
{
    public sealed record GetQuizAnswerByAttemptIdQuery(long attemptId, long questionId) 
        : IRequest<GetQuizAnswerByAttemptIdResult>
    {
    }
}