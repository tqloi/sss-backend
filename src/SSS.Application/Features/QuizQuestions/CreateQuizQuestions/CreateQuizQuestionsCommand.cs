using MediatR;
using SSS.Application.Features.QuizQuestions.Common;

namespace SSS.Application.Features.QuizQuestions.CreateQuizQuestions
{
    public sealed record CreateQuizQuestionsCommand(List<CreateQuizQuestionWithOptionsDto> CreateQuizQuestionDtos)
        : IRequest<CreateQuizQuestionsResult>;
}
