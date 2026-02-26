using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAnswers.Common;
using SSS.Application.Features.QuizQuestions.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizQuestions.GetQuizQuestionById
{
    public class GetQuizQuestionByIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizQuestionByIdQuery, GetQuizQuestionByIdResult>
    {
        public async Task<GetQuizQuestionByIdResult> Handle(GetQuizQuestionByIdQuery req, CancellationToken cancellationToken)
        {
            var quizQuestion = await db.QuizQuestions
                .FirstOrDefaultAsync(q => q.Id == req.id);
            if (quizQuestion is null)
            {
                throw new Exception($"Quiz question with id {req.id} not found.");
            }
            var result =  mapper.Map<QuizQuestionDto>(quizQuestion);

            return new GetQuizQuestionByIdResult(result);
        }
    }
}
