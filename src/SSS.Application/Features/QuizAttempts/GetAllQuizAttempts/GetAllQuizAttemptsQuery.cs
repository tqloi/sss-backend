using MediatR;

namespace SSS.Application.Features.QuizAttempts.GetAllQuizAttempts
{
    public sealed record GetAllQuizAttemptsQuery : IRequest<GetAllQuizAttemptsResult>
    {
        public int PageIndex { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}