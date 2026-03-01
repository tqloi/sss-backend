using MediatR;

namespace SSS.Application.Features.QuizQuestionOptions.GetQuizQuestionOptionById
{
    public sealed record GetQuizQuestionOptionByIdQuery(long Id)
        : IRequest<GetQuizQuestionOptionByIdResult>
    {
    }
}