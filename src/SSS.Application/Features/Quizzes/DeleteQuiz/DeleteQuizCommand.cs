using MediatR;

namespace SSS.Application.Features.Quizzes.DeleteQuiz
{
    public sealed record DeleteQuizCommand(long QuizId) 
        : IRequest<DeleteQuizResult>;
}