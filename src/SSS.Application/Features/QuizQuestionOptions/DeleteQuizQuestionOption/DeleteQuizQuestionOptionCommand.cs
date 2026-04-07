using MediatR;

namespace SSS.Application.Features.QuizQuestionOptions.DeleteQuizQuestionOption
{
    public sealed record DeleteQuizQuestionOptionCommand(long Id)
        : IRequest<DeleteQuizQuestionOptionResult>
    {
    }
}