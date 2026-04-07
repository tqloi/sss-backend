using MediatR;

namespace SSS.Application.Features.QuizQuestions.GetAllQuizQuestionsByQuizId
{
    public sealed record GetAllQuizQuestionsByQuizIdQuery(long quizId) 
        : IRequest<GetAllQuizQuestionsByQuizIdResult>
    {
    }
}