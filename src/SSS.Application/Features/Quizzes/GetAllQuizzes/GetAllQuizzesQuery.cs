using MediatR;

namespace SSS.Application.Features.Quizzes.GetAllQuizzes
{
    public sealed record GetAllQuizzesQuery : IRequest<GetAllQuizzesResult>
    {
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}