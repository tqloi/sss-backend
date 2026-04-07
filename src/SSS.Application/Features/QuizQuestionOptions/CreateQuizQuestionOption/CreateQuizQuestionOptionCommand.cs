using MediatR;
using SSS.Application.Features.QuizQuestionOptions.Common;

namespace SSS.Application.Features.QuizQuestionOptions.CreateQuizQuestionOption
{
    public sealed record CreateQuizQuestionOptionCommand(CreateQuizQuestionOptionDto CreateQuizQuestionOptionDto)
        : IRequest<CreateQuizQuestionOptionResult>
    {
    }
}