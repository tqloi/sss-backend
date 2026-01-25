using MediatR;

namespace SSS.Application.Features.QuizAnswers.GetQuizAnswerById
{
    public sealed record GetQuizAnswerByIdQuery(long id)
        : IRequest<GetQuizAnswerByIdResponse>
    {
    }
}