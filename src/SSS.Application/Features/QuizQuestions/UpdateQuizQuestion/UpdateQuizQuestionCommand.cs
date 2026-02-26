using MediatR;
using SSS.Application.Features.QuizQuestions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.UpdateQuizQuestion
{
    public sealed record UpdateQuizQuestionCommand(long Id, UpdateQuizQuestionDto UpdateQuizQuestionDto)
        : IRequest<UpdateQuizQuestionResult>
    {
    }
}
