using MediatR;
using SSS.Application.Features.QuizQuestions.Common;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestion
{
    public sealed record CreateQuizQuestionCommand(CreateQuizQuestionDto CreateQuizQuestionDto)
        : IRequest<CreateQuizQuestionResult>
    {
    }
}
