using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.DeleteQuizAnswers
{
    public sealed record DeleteQuizAnswersCommand(long id)
        : IRequest<DeleteQuizAnswersResult>
    {
    }
}
