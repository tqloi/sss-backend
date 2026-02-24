using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using SSS.Application.Features.QuizAnswers.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAnswers.GetQuizAnswerById
{
    public class GetQuizAnswerByIdHandler(IAppDbContext db, IMapper mapper) 
        : IRequestHandler<GetQuizAnswerByIdQuery, GetQuizAnswerByIdResult>
    {
        public async Task<GetQuizAnswerByIdResult> Handle(GetQuizAnswerByIdQuery request, CancellationToken cancellationToken)
        {
            var quizAnswer = await db.QuizAnswers
                .AsNoTracking()
                .Include(q => q.Question)
                .Include(a => a.Attempt)
                .Include(op => op.Option)
                .FirstOrDefaultAsync(qa => qa.Id == request.id, cancellationToken);

            if (quizAnswer is null)
            {
                return new GetQuizAnswerByIdResult(null);
            }
            var quizAnswerDto = mapper.Map<QuizAnswerDto>(quizAnswer);
            return new GetQuizAnswerByIdResult(quizAnswerDto);
        }
    }
}
