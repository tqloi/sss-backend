using MediatR;

namespace SSS.Application.Features.Quizzes.GetQuizById
{
    public sealed record GetQuizByIdQuery(long id)
        : IRequest<GetQuizByIdResult>
    {
    }
}