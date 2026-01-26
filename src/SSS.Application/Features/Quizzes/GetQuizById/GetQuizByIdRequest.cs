using MediatR;

namespace SSS.Application.Features.Quizzes.GetQuizById
{
    public sealed record GetQuizByIdRequest(long id)
        : IRequest<GetQuizByIdResponse>
    {
    }
}