using MediatR;

namespace SSS.Application.Features.Quizzes.DeleteQuiz
{
    public sealed record DeleteQuizRequest(long QuizId) 
        : IRequest<DeleteQuizResponse>;
}