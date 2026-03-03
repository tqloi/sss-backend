using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SSS.Application.Abstractions.Persistence.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSS.Application.Features.QuizAttempts.GetQuizAttemptById
{
    public class GetQuizAttemptByIdHandler(IAppDbContext db, IMapper mapper)
        : IRequestHandler<GetQuizAttemptByIdQuery, GetQuizAttemptByIdResult>
    {
        public async Task<GetQuizAttemptByIdResult> Handle(GetQuizAttemptByIdQuery req, CancellationToken cancellationToken)
        {          
            var quizAttempt = await db.QuizAttempts.FirstOrDefaultAsync(q => q.Id == req.Id);
            if (quizAttempt is null)
            {
                throw new KeyNotFoundException($"Quiz attempt with id {req.Id} not found.");
            }
            var quizAttemptDto = mapper.Map<Common.QuizAttemptDto>(quizAttempt);
            return new GetQuizAttemptByIdResult(quizAttemptDto);
        }
    }
}
