using MediatR;
using SSS.Application.Features.Quizzes.Common;

namespace SSS.Application.Features.Quizzes.CreateQuiz
{
    public sealed record CreateQuizRequest(CreateQuizDto CreateQuizNode)
        : IRequest<CreateQuizResponse>;
}