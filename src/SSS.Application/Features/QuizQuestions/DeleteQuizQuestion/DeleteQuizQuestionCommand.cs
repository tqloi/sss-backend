using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.DeleteQuizQuestion
{
    public sealed record DeleteQuizQuestionCommand(long id)
        : IRequest<DeleteQuizQuestionResult>
    {
    }
}
