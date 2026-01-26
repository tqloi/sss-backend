using MediatR;

namespace SSS.Application.Features.Quizzes.GetAllQuizNode
{
    public sealed record GetAllQuizzesRequest : IRequest<GetAllQuizzesResponse>
    {
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}