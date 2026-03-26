using MediatR;
using SSS.Application.Features.QuizAnswers.Common;

namespace SSS.Application.Features.QuizAnswers.SaveQuizAnswersByAttemptId
{
    public sealed record SaveQuizAnswersByAttemptIdCommand(
        long AttemptId,
        List<SaveQuizAnswerByQuestionDto> QuizAnswers)
        : IRequest<SaveQuizAnswersByAttemptIdResult>;
}
