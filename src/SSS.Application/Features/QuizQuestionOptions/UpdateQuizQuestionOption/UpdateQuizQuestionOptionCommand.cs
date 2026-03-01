using MediatR;
using SSS.Application.Features.QuizQuestionOptions.Common;

namespace SSS.Application.Features.QuizQuestionOptions.UpdateQuizQuestionOption
{
    public sealed record UpdateQuizQuestionOptionCommand(long Id, UpdateQuizQuestionOptionDto UpdateQuizQuestionOptionDto)
        : IRequest<UpdateQuizQuestionOptionResult>
    {
    }
}